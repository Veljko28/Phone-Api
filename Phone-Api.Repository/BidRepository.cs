using Microsoft.Extensions.Configuration;
using Phone_Api.Models;
using Phone_Api.Models.BidModels;
using Phone_Api.Models.Requests.BidRequests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
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

		public async Task<GenericResponse> AddBidAsync(BidRequest req)
		{
			BidModel model = new BidModel
			{
				Id = Guid.NewGuid().ToString(),
				Image = req.Image,
				Name = req.Name,
				Description = req.Description,
				Price = req.Price,
				Brand = req.Brand,
				Category = req.Category,
				Seller = req.Seller,
				TimeCreated = req.TimeCreated,
				TimeEnds = req.TimeEnds
			};

			string sql = "exec [_spAddBid] @Id, @Image, @Name, @Description, @Price, @Brand, @Category, @Seller, @TimeCreated, @TimeEnds";

			return await DatabaseOperations.GenericExecute(sql, model, _configuration, "Failed to add bid to database");
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

		public async Task<BidRequest> GetBidByIdAsync(string bid_Id)
		{
			string sql = "exec [_spGetBidById] @Id";

			return await DatabaseOperations.GenericQuerySingle<dynamic, BidRequest>(sql, new { Id = bid_Id }, _configuration);
		}
	}
}
