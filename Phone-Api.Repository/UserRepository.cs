using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Models.ReviewModels;
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

		public async Task<UserModel> GetUserByIdAsync(string Id)
		{
			string sql = "exec [_spGetUserById] @Id";
			return await DatabaseOperations.GenericQuerySingle<dynamic, UserModel>(sql, new { Id }, _configuration);
		}

		public async Task<GenericResponse> ChangePasswordAsync(string userId, ChangePasswordRequest change)
		{
			if (change.Current_Password != change.Confirm_Current_Password)
				return new GenericResponse
				{
					Success = false,
					ErrorMessage = "The Passwords do not match"
				};

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				UserModel user = await GetUserByIdAsync(userId);

				db.Close();

				if (user == null) return new GenericResponse { Success = false, ErrorMessage = "Cannot find user !"};

				string dbPassword = user.Password;

				if (dbPassword != change.Current_Password)
				{
					if (!PasswordHashing.ComparePasswords(dbPassword, change.Current_Password))
							return new GenericResponse { Success = false, ErrorMessage = "Incorrect Password !" };
				}

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

		public async Task<string> RegisterAsync(UserRequest userRequest)
		{
			string sql = "exec [_spRegisterUser] @Id, @Email, @UserName, @Password";

			if (userRequest.Confirm_Password != userRequest.Password)
				return null;

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				// Check if the email is in use

				string EmailInUse = (await db.QueryAsync<string>("exec [_spCheckInUseEmail] @Email", new { userRequest.Email })).FirstOrDefault();

				if (EmailInUse != null)
				{
					return null;

				}

				// Check if the username is in use

				string UserNameInUse = (await db.QueryAsync<string>("exec [_spCheckInUseUserName] @UserName", new { userRequest.UserName })).FirstOrDefault();

				if (UserNameInUse != null)
				{
					return null;
				}

			}

			string passwordHash = PasswordHashing.HashPassword(userRequest.Password);

			UserModel user = new UserModel
			{
				Id = Guid.NewGuid().ToString(),
				Email = userRequest.Email,
				Password = passwordHash,
				UserName = userRequest.UserName,
			};

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, user);

				if (rowsModified > 0)
				{
					return user.Id;
				}

				return null;

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

				UserModel currentUser = await GetUserByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

				if (currentUser == null)
				{
						return null;
				}
				else return await LoginAsync(new LoginRequest { 
						Email_UserName = currentUser.Email,
						Password = currentUser.Password
				});
			}

		public async Task<GenericResponse> EditUserProfileAsync(string userId, EditProfileModel model)
		{
			string sql = "exec [_spEditUserProfile] @Id, @UserName, @Email, @Description, @PhoneNumber";

			return await DatabaseOperations.GenericExecute(sql, new { Id = userId, model.UserName, model.Email, model.Description, model.PhoneNumber }, _configuration, "Failed to update the profile");
		}

		public async Task<string> GetUserNameByIdAsync(string userId)
		{
			string sql = "exec [_spGetUserNameById] @Id";

			return await DatabaseOperations.GenericQuerySingle<dynamic, string>(sql, new { Id = userId }, _configuration);
		}

		public async Task<UserModel> GetUserByEmailAsync(string email)
		{
			string sql = "exec [_spGetUserByEmail] @Email";

			return await DatabaseOperations.GenericQuerySingle<dynamic, UserModel>(sql, new { Email = email }, _configuration);
		}

		public async Task<string> GetEmailByIdAsync(string userId)
		{
			string sql = "exec [_spGetUserEmailById] @Id";

			return await DatabaseOperations.GenericQuerySingle<dynamic, string>(sql, new { Id = userId }, _configuration);
		}

		public async Task<bool> UpdatePhonesSoldAsync(string userId)
		{
			string sql = "UPDATE [dbo].[Users] SET Phones_sold = Phones_sold + 1 WHERE Id = '" + userId + "'";


			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql);

				return rowsModified > 0;

			}
		}

		public async Task<string> GetUserIdByNameAsync(string userName)
		{
			string sql = "exec [_spGetUserIdByName] @UserName";

			return await DatabaseOperations.GenericQuerySingle<dynamic, string>(sql, new { UserName = userName }, _configuration);
		}

		public async Task<GenericResponse> AddLoyalityPointsAsync(LoyalityPointsRequest request)
		{
			if (request.Amount == 0)
			{
				string sql = "exec [_spAddLoyalityPoints] @Id";

				return await DatabaseOperations.GenericExecute(sql, new { Id = request.UserId }, _configuration, "Failed to add loyality points");
			}
			else
			{
				string sql = "exec [_spAddLoyalityPointsAmount] @Id, @Amount";

				return await DatabaseOperations.GenericExecute(sql, new { Id = request.UserId, request.Amount }, _configuration, "Failed to add loyality points");
			}
		}

		public async Task<int> GetLoyalityPointsAsync(string userId)
		{
			string sql = "exec [_spGetLoyalityPoints] @Id";

			return await DatabaseOperations.GenericQuerySingle<dynamic, int>(sql, new { Id = userId }, _configuration);
		}

		public async Task<GenericResponse> RemoveLoyalityPointsAsync(LoyalityPointsRequest request)
		{
			int userPoints = await GetLoyalityPointsAsync(request.UserId);

			if (request.Amount > userPoints) return new GenericResponse { Success = false, ErrorMessage = "You don't have enough points for this purchase" };

			string sql = "exec [_spRemoveLoyalityPoints] @Id, @Amount";

			return await DatabaseOperations.GenericExecute(sql, new { Id = request.UserId, request.Amount }, _configuration, "Failed to remove loyality points");
		}

		public async Task<GenericResponse> AddUserCouponAsync(CouponModel model)
		{
			string sql = "exec [_spAddUserCoupon] @UserId, @Coupon, @Amount";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to add coupon the database");
		}

		public async Task<IEnumerable<ReviewModel>> GetUserReviewsAsync(string userId)
		{
			string sql = "exec [_spGetUserReviews] @SellerId";

			return await DatabaseOperations.GenericQueryList<dynamic, ReviewModel>(sql, new { SellerId = userId }, _configuration);
		}

		public async Task<GenericResponse> UpdateRatingAsync(double rating, string userId)
		{
			string sql = "exec [_spUpdateUserRating] @UserId, @Rating";

			return await DatabaseOperations.GenericExecute(sql, new { UserId = userId, Rating = rating }, _configuration, "Failed to update rating");
		}
	}
}
