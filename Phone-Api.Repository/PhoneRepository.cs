using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
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
		public async Task<PhoneModel> AddPhoneAsync(PhoneRequest phoneRequest, string userId)
		{

			PhoneModel phone = new PhoneModel
			{
				Id = Guid.NewGuid().ToString(),
				Seller = userId,
				Image = phoneRequest.Image,
				Price = phoneRequest.Price,
				Description = phoneRequest.Description,
				Name = phoneRequest.Name,
				Category = phoneRequest.Category,
				Brand = phoneRequest.Brand,
				Status = PhoneStatus.Running
			};

			string sql = "exec [_spAddPhone] @Id, @Image, @Seller, @Price, @Description, @Name, @Category, @Brand, @Status";

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

		public async Task<GenericResponse> DeletePhoneAsync(string phoneId)
		{
			string sql = "exec [_spDeletePhone] @Id";

			return await DatabaseOperations.GenericExecute(sql, new { Id = phoneId }, _configuration, "Failed to delete the phone");
		}

		public async Task<GenericResponse> EditPhoneAsync(PhoneModel model)
		{
			string sql = "exec [_spEditPhone] @Id, @Image, @Name, @Description, @Price, @Seller, @Category, @DateCreated, @Brand, @Status";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to update the phone");
		}

		public async Task<IEnumerable<PhoneModel>> GetFeaturedPhonesAsync()
		{
			string sql = "SELECT TOP 4 * FROM [dbo].[Phones] ORDER BY NEWID()";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var res = await db.QueryAsync<PhoneModel>(sql);

				return res;
			}
		}

		public async Task<IEnumerable<PhoneModel>> GetLastestPhonesAsync()
		{
			string sql = "SELECT TOP 4 * FROM [dbo].[Phones]";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var res = await db.QueryAsync<PhoneModel>(sql);

				return res;
			}
		}

		public async Task<PhoneModel> GetPhoneByIdAsync(string Id)
		{
			string sql = "exec [_spFindPhoneByID] @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var phone = (await db.QueryAsync<PhoneModel>(sql, new { Id })).FirstOrDefault();

				return phone;
			}

		}

		public async Task<IEnumerable<string>> GetPhoneImagesAsync(string phoneId)
		{
			string sql = "exec [_spGetPhoneImages] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, string>(sql, new { Id = phoneId }, _configuration);
		}

		public async Task<IEnumerable<PhoneModel>> GetPhonePageAsync(string page)
		{
			string sql = "exec [_spGetPhonePage] @Page";

			return await DatabaseOperations.GenericQueryList<dynamic, PhoneModel>(sql, new { Page = page }, _configuration);
		}

		public async Task<IEnumerable<PhoneModel>> GetSellerPhonesByIdAsync(string sellerId)
		{
			string sql = "exec [_spSellerPhonesById] @Id";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var phones = await db.QueryAsync<PhoneModel>(sql, new { Id = sellerId});

				return phones;
			}
		}
	}
}
