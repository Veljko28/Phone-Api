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
	public class PhoneRepository : IPhoneRepository
	{
		private readonly IConfiguration _configuration;
		public PhoneRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<PhoneModel> addPhoneAsync(PhoneRequest phoneRequest, string userId)
		{

			PhoneModel phone = new PhoneModel
			{
				Id = Guid.NewGuid().ToString(),
				Seller = userId,
				Name = phoneRequest.Name,
				Image = phoneRequest.Image,
				Category = phoneRequest.Category,
				Brand = phoneRequest.Brand

			};

			string sql = "exec _spAddPhone @Id @Seller @Name @Image @Category @Brand";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection") ))
			{
				await db.OpenAsync();

				int modified = await db.ExecuteAsync(sql, phone);

				if (modified > 0)
				{
					return phone;
				}
			}

			return null;
		}

		public async Task<PhoneModel> getPhoneByIdAsync(string Id)
		{
			string sql = "exec _spFindPhoneByID @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var phone = (await db.QueryAsync<PhoneModel>(sql, new { Id })).FirstOrDefault();

				return phone;
			}

		}
	}
}
