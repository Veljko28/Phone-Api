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

		public async Task<GenericResponse> ChangeStatusAsync(ChangePhoneStatusRequest request)
		{
			string sql = "exec [_spChangePhoneStatus] @Id, @Status";

			return await DatabaseOperations.GenericExecute(sql, new { Id = request.PhoneId, request.Status }, _configuration, "Failed to change status");
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

		public async Task<IEnumerable<PhoneModel>> GetFeaturedPhonesAsync(string phoneId)
		{
			string sql = "SELECT TOP 4 * FROM [dbo].[Phones] WHERE Status = 0 AND Id != '" + phoneId + "' ORDER BY NEWID()";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var res = await db.QueryAsync<PhoneModel>(sql);

				return res;
			}
		}

		public async Task<IEnumerable<PhoneModel>> GetLastestPhonesAsync()
		{
			string sql = "SELECT TOP 4 * FROM [dbo].[Phones] WHERE Status = 0";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var res = await db.QueryAsync<PhoneModel>(sql);

				return res;
			}
		}

		public async Task<int> GetNumOfPagesAsync(string sellerId = null)
		{
			string sql = "SELECT COUNT(*) FROM [dbo].[Phones] WHERE Status = 0";

			if (sellerId != null)
			{
			    sql = "SELECT COUNT(*) FROM [dbo].[Phones] WHERE Seller = '" + sellerId + "'";
			}

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int numOfPages = await db.ExecuteScalarAsync<int>(sql);

				if (sellerId != null)
				{
					double managementPages = numOfPages / 8.0;

					return (int)Math.Ceiling(managementPages);
				}

				double pages = numOfPages / 10.0;

				return (int)Math.Ceiling(pages);
			}
		}

		public async Task<int> GetNumOfUserPhonesAsync(string userId)
		{
			string sql = "exec [_spGetNumberOfUserPhones] @UserId";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int numOfPhones = await db.ExecuteScalarAsync<int>(sql, new { UserId = userId });

				return numOfPhones;
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

		public async Task<IEnumerable<ReviewModel>> GetPhoneReviewsById(string Id)
		{
			string sql = "exec [_spGetPhoneReviewsById] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, ReviewModel>(sql, new { Id }, _configuration);
		}

		public async Task<IEnumerable<PhoneModel>> GetSellerPhonesByIdAsync(string sellerId, int pageNum)
		{
			string sql = "exec [_spSellerPhonesById] @Id, @Page";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				var phones = await db.QueryAsync<PhoneModel>(sql, new { Id = sellerId, Page = pageNum});

				return phones;
			}
		}
	}
}
