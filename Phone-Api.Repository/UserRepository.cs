using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

		public async Task<UserModel> LoginAsync(LoginRequest loginRequest)
		{
			string sql = "exec _spLoginUser @Email_UserName @Password";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				UserModel user = (await db.QueryAsync<UserModel>(sql, loginRequest)).FirstOrDefault();

				return user;
			}

		}

		public async  Task<bool> RegisterAsync(UserRequest userRequest)
		{
			string sql = "exec _spRegisterUser @Email @UserName @Password @Confirm_Password";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, userRequest);

				return rowsModified > 0;
			}
		}
	}
}
