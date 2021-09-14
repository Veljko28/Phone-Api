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
using System.Linq;
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
				Date_Ends = req.Date_Ends,
				Status = BidStatus.Running
			};

			string sql = "exec [_spAddBid] @Id, @Name, @Image, @Description, @Price, @Brand, @Category, @Seller, @TimeCreated, @Date_Ends, @Status";

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

		public async Task<IEnumerable<BidModel>> GetBidsAsync()
		{
			string sql = "exec [_spGetAllBids]";

			return await DatabaseOperations.GenericQueryList<dynamic, BidModel>(sql, new {  }, _configuration);
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

		public async Task<int> GetNumOfPagesAsync(string sellerId = null)
		{
			string sql = "SELECT COUNT(*) FROM [dbo].[Bids]";

			if (sellerId != null)
			{
				sql = "SELECT COUNT(*) FROM [dbo].[Bids] WHERE Seller = '" + sellerId + "'";
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

		public async Task<List<BidModel>> GetPlacedBidsAsync(string userName, int page)
		{
			string sql = "exec [_spGetBidHisotriesByUserName] @UserName, @Page";

			var histories = await DatabaseOperations.GenericQueryList<dynamic, BidHistoryModel>(sql, new { UserName = userName, Page = page }, _configuration);

			List<BidModel> bids = new List<BidModel>();
			List<string> bidIds = new List<string>();

			foreach (var history in histories)
			{
				BidModel bid = await GetBidByIdAsync(history.Bid_Id);

				if (bid != null && !bidIds.Contains(history.Bid_Id)) bids.Add(bid);

				bidIds.Add(history.Bid_Id);
			}

			return bids;
		}

		public async Task<int> GetPlacedBidsNumOfPagesAsync(string userName)
		{
			string sql = "exec [_spGetPlacedBidNumOfPages] @userName";

			using (SqlConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				await db.OpenAsync();

				int numOfPages = await db.ExecuteScalarAsync<int>(sql,new { UserName = userName });

				double managementPages = numOfPages / 8.0;

				return (int)Math.Ceiling(managementPages);
			}
		}

		public async Task<string> ChangeStatusAsync(ChangeBidStatusRequest bidRequest)
		{
			string sql = "exec [_spChangeBidStatus] @Id, @Status";

			var histories = await GetBidHistoriesAsync(bidRequest.Bid_Id);

			if (histories.Count() == 0)
			{
				bidRequest.Status = BidStatus.Failed;
				GenericResponse response = await DatabaseOperations.GenericExecute(sql, new { Id = bidRequest.Bid_Id, bidRequest.Status }, _configuration, "Failed to update the status");
			}
			else
			{
			
				decimal highest_value = histories.Max(x => x.Amount);

				BidHistoryModel highest_bid = histories.Where(x => x.Amount == highest_value).FirstOrDefault();

				if (highest_bid == null)
				{
					bidRequest.Status = BidStatus.Failed;
				}

				string userName = highest_bid?.UserName;

				GenericResponse response = await DatabaseOperations.GenericExecute(sql, new { Id = bidRequest.Bid_Id, bidRequest.Status}, _configuration, "Failed to update the status");

				if (response.Success)
				{
					return userName;
				}
			}

			return null;
		}
	}
}
