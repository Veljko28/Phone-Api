using Phone_Api.Models;
using Phone_Api.Models.BidModels;
using Phone_Api.Models.Requests.BidRequests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Repository.Interfaces
{
	public interface IBidRepository
	{
		Task<BidModel> AddBidAsync(BidRequest req);
		Task<BidRequest> GetBidByIdAsync(string bid_Id);
		Task<IEnumerable<BidHistoryModel>> GetBidHistoriesAsync(string bid_Id);
		Task<GenericResponse> AddToBidHistoryAsync(BidHistoryRequest req);
		Task<IEnumerable<BidModel>> GetUserBidsAsync(string userId);
	}
}
