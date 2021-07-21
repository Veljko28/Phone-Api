using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly IConfiguration _configuration;
		public UserRepository(IConfiguration configuration)
		{
			_configuration = configuration;
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

		public async Task<UserModel> LoginAsync(LoginRequest loginRequest)
		{
			string passwordSql = "exec [_spLoginUser] @Email_UserName";

			UserModel user = new UserModel();

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				user = (await db.QueryAsync<UserModel>(passwordSql, new { loginRequest.Email_UserName })).FirstOrDefault();

			}

			if (user == null || user.Password == null) return null;

			if (!PasswordHashing.ComparePasswords(user.Password, loginRequest.Password))
			{
				return null;
			}

			return user;

		}

		public async Task<GenericResponse> RegisterAsync(UserRequest userRequest)
		{
			string sql = "exec [_spRegisterUser] @Id, @Email, @UserName, @Password";

			if (userRequest.Confirm_Password != userRequest.Password)
				return new GenericResponse {
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
	}
}
