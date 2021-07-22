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

		public async Task<GenericResponse> AddPhoneToPurchaseAsync(string PurchaseId, string PhoneId)
		{
			string sql = "exec [_spAddPhoneToPurchase] @PurchaseId, @PhoneId";

			return await DatabaseOperations.GenericExecute(sql, new { PurchaseId , PhoneId }, _configuration, "Failed to add phone to purchase list");
		}

		public async Task<string> AddPurchaseAsync(PurchaseRequest req)
		{
			string sql = "exec [_spAddPurchase] @Id, @UserId, @Total, @PurchaseDate";

			PurchaseModel model = new PurchaseModel
			{
				Id = Guid.NewGuid().ToString(),
				UserId = req.UserId,
				Total = req.Total,
				PurchaseDate = req.PurchaseDate
			};

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int rowsModified = await db.ExecuteAsync(sql, model);

				if (rowsModified > 0) return model.Id;

				return null;
			}
		}

		public async Task<IEnumerable<string>> GetPurchasePhonesAsync(string PurchaseId)
		{
			string sql = "exec [_spGetPurchasedPhones] @PurchaseId";

			return await DatabaseOperations.GenericQueryList<dynamic, string>(sql, new { PurchaseId }, _configuration);
		}
	}
}
