using Dapper;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.BidModels;
using Phone_Api.Models.Requests.BidRequests;
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
	public class BidRepository : IBidRepository
	{
		private readonly IConfiguration _configuration;

		public BidRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<BidModel> AddBidAsync(BidRequest req, string userId)
		{
			BidModel model = new BidModel
			{
				Id = Guid.NewGuid().ToString(),
				Name = req.Name,
				Image = req.Image,
				Description = req.Description,
				Price = req.Price,
				Brand = req.Brand,
				Category = req.Category,
				Seller = userId,
				TimeCreated = req.TimeCreated,
				TimeEnds = req.TimeEnds,
				Status = BidStatus.Running
			};

			string sql = "exec [_spAddBid] @Id, @Name, @Image, @Description, @Price, @Brand, @Category, @Seller, @TimeCreated, @TimeEnds, @Status";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int modified = await db.ExecuteAsync(sql, model);

				if (modified > 0)
				{
					return model;
				}
			}

			return null;
		}

		public async Task<GenericResponse> AddToBidHistoryAsync(BidHistoryRequest req)
		{
			BidHistoryModel model = new BidHistoryModel
			{
				Id = Guid.NewGuid().ToString(),
				Bid_Id = req.Bid_Id,
				UserName = req.UserName,
				Amount = req.Amount
			};

			string sql = "exec [_spAddBidHistory] @Id, @Bid_Id, @UserName, @Amount";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to add bid history to database");
		}

		public async Task<IEnumerable<BidHistoryModel>> GetBidHistoriesAsync(string bid_Id)
		{
			string sql = "exec [_spGetBidHistories] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, BidHistoryModel>(sql, new { Id = bid_Id }, _configuration);
		}

		public async Task<BidModel> GetBidByIdAsync(string bid_Id)
		{
			string sql = "exec [_spGetBidById] @Id";

			return await DatabaseOperations.GenericQuerySingle<dynamic, BidModel>(sql, new { Id = bid_Id }, _configuration);
		}

		public async Task<IEnumerable<BidModel>> GetUserBidsAsync(string userId)
		{
			string sql = "exec [_spGetUserBids] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, BidModel>(sql, new { Id = userId }, _configuration);
		}

		public async Task<IEnumerable<BidModel>> GetBidPageAsync(string pageId)
		{
			string sql = "exec [_spGetBidPage] @Page";

			return await DatabaseOperations.GenericQueryList<dynamic, BidModel>(sql, new { Page = pageId }, _configuration);
		}

		public async Task<IEnumerable<string>> GetBidImagesAsync(string bid_Id)
		{
			string sql = "exec [_spGetBidImages] @Id";

			return await DatabaseOperations.GenericQueryList<dynamic, string>(sql, new { Id = bid_Id }, _configuration);
		}

		public async Task<GenericResponse> DeleteBidAsync(string bid_Id)
		{
			string sql = "exec [_spDeleteBid] @Id";

			return await DatabaseOperations.GenericExecute(sql, new { Id = bid_Id }, _configuration, "Failed to delete the bid");
		}

		public async Task<GenericResponse> UpdatePriceAsync(BidPriceUpdateRequest req)
		{
			string sql = "exec [_spUpdateBidPrice] @Id, @Price";

			return await DatabaseOperations.GenericExecute(sql, req, _configuration, "Failed to update the price");
		}
	}
}
