using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository
{
	public class PurchaseRepository : IPurchaseRepository
	{
		private readonly IConfiguration _configuration;

		public PurchaseRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<GenericResponse> AddPurchaseAsync(IEnumerable<PurchaseRequest> req)
		{
			string sql = "exec [_spAddPurchase] @BuyerId, @SellerId, @PhoneId";


			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				foreach(var model in req)
				{
					int rowsModified = await db.ExecuteAsync(sql, model);

					if (rowsModified == 0)
					{
						return new GenericResponse { Success = false, ErrorMessage = "Failed to add a purchase" };
					}

				}
				return new GenericResponse { Success = true };
			}
		}

		public async Task<int> GetNumOfPagesAsync(string userId)
		{
			string sql = "SELECT COUNT(*) FROM [dbo].[PhonePurchases] WHERE BuyerId = '" + userId + "'";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int numOfPages = await db.ExecuteScalarAsync<int>(sql);

				double managementPages = numOfPages / 8.0;

				return (int)Math.Ceiling(managementPages);

			}
		}

		public async Task<IEnumerable<string>> GetPurchasedPhonesPageAsync(string userId, int page)
		{
			string sql = "exec [_spGetPurchasedPhones] @UserId, @Page";

			return await DatabaseOperations.GenericQueryList<dynamic,string>(sql, new { UserId = userId, Page = page }, _configuration);
		}
	}
}
