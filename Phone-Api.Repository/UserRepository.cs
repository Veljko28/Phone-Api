using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly IConfiguration _configuration;
		private readonly JwtSettings _jwtSettings;
		private readonly IRefreshTokenRepository _tokenDb;

		public UserRepository(IConfiguration configuration, JwtSettings jwtSettings, IRefreshTokenRepository tokenDb)
		{
			_configuration = configuration;
			_jwtSettings = jwtSettings;
			_tokenDb = tokenDb;
		}

		public async Task<GenericResponse> ChangePasswordAsync(string userId, ChangePasswordRequest change)
		{
			if (change.Old_Password != change.Confirm_Old_Password)
				return new GenericResponse
				{
					Success = false,
					ErrorMessage = "The Passwords do not match"
				};

			string findUserPassword = "exec [_spGetUserById] @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				UserModel user = (await db.QueryAsync<UserModel>(findUserPassword, new { Id = userId })).FirstOrDefault();

				db.Close();

				if (user == null) return new GenericResponse { Success = false, ErrorMessage = "Cannot find user !"};

				string dbPassword = user.Password;

				if (!PasswordHashing.ComparePasswords(dbPassword, change.Old_Password))
						return new GenericResponse { Success = false, ErrorMessage = "Incorrect Password !" };

				string passwordHash = PasswordHashing.HashPassword(change.New_Password);

				await db.OpenAsync();

				int rowsModified  = await db.ExecuteAsync("exec [_spChangePassword] @Id, @Password", new { Id = userId, Password = passwordHash });

				if (rowsModified > 0) return new GenericResponse { Success = true };

				return new GenericResponse { Success = false, ErrorMessage = "We are exipirencing problems on the server. Try again later" };
			}
		}

		public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
		{
			string passwordSql = "exec [_spLoginUser] @Email_UserName";

			UserModel user = new UserModel();

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				user = (await db.QueryAsync<UserModel>(passwordSql, new { loginRequest.Email_UserName })).FirstOrDefault();

			}

			if (user == null || user.Password == null) return null;

			if (user.Password != loginRequest.Password && !PasswordHashing.ComparePasswords(user.Password, loginRequest.Password))
			{
				return null;
			}


			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

			List<Claim> roleClaims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("id", user.Id),
			};


			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(roleClaims),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);


			var refreshToken = new RefreshToken
			{
				Token = tokenHandler.WriteToken(token),
				JwtId = token.Id,
				UserId = user.Id,
				CreatedDate = DateTime.UtcNow,
				Expires = DateTime.UtcNow.AddMonths(6),

			};

			bool addedToken = await _tokenDb.AddTokenAsync(refreshToken);

			if (!addedToken)
			{
				return null;
			}

			return new TokenResponse
			{
				Token = tokenHandler.WriteToken(token),
				RefreshToken = refreshToken.JwtId,
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime).ToString()
			};

		}

		public async Task<GenericResponse> RegisterAsync(UserRequest userRequest)
		{
			string sql = "exec [_spRegisterUser] @Id, @Email, @UserName, @Password";

			if (userRequest.Confirm_Password != userRequest.Password)
				return new GenericResponse
				{
					Success = false,
					ErrorMessage = "The Passwords do not match !"
				};

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				// Check if the email is in use

				string EmailInUse = (await db.QueryAsync<string>("exec [_spCheckInUseEmail] @Email", new { userRequest.Email })).FirstOrDefault();

				if (EmailInUse != null)
				{
					return new GenericResponse
					{
						Success = false,
						ErrorMessage = "This Email is already in use !"
					};
				}

				// Check if the username is in use

				string UserNameInUse = (await db.QueryAsync<string>("exec [_spCheckInUseUserName] @UserName", new { userRequest.UserName })).FirstOrDefault();

				if (UserNameInUse != null)
				{
					return new GenericResponse
					{
						Success = false,
						ErrorMessage = "This Username is already in use !"
					};
				}

			}

			string passwordHash = PasswordHashing.HashPassword(userRequest.Password);

			UserModel user = new UserModel
			{
				Id = Guid.NewGuid().ToString(),
				Email = userRequest.Email,
				Password = passwordHash,
				UserName = userRequest.UserName
			};

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, user);

				if (rowsModified > 0)
				{
					return new GenericResponse
					{
						Success = true
					};
				}

				return new GenericResponse
				{
					Success = false,
					ErrorMessage = "Problem with registering the user"
				};

			}
		}


			public async Task<TokenResponse> RefreshTokenAsync(string token, string refreshToken)
			{
				var validatedToken = JwtValidation.getPrincipalFromToken(token, _configuration);

				if (validatedToken == null)
				{
					return null;
				}

				var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

				var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

				if (expiryDateTimeUtc > DateTime.UtcNow)
				{
					return null;
				}

				var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;


				var storedRefreshToken = await _tokenDb.FindTokenAsync(token);

				if (storedRefreshToken == null || DateTime.UtcNow > storedRefreshToken.Expires || storedRefreshToken.Invalidated ||
					storedRefreshToken.Used || storedRefreshToken.JwtId != jti)
				{
					return null;
				}


				storedRefreshToken.Used = true;
				bool result = await _tokenDb.UpdateTokenAsync(storedRefreshToken);

				if (!result)
				{
					return null;
				}

				string sql = "exec [_spGetUserById] @Id";
				UserModel currentUser = await DatabaseOperations.GenericQuerySingle
				<dynamic, UserModel>(sql ,new { Id = validatedToken.Claims.Single(x => x.Type == "id").Value },_configuration);

				if (currentUser == null)
				{
					return null;
				}
				else return await LoginAsync(new LoginRequest { 
					Email_UserName = currentUser.Email,
					Password = currentUser.Password
				});
			}

	}
}
