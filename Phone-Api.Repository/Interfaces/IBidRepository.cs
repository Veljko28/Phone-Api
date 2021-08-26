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
		Task<int> GetNumOfPagesAsync(string sellerId = null);
		Task<int> GetPlacedBidsNumOfPagesAsync(string userName);
		Task<BidModel> GetBidByIdAsync(string bid_Id);
		Task<BidModel> AddBidAsync(BidRequest req, string userId);
		Task<List<BidModel>> GetPlacedBidsAsync(string userName, int page);
		Task<IEnumerable<BidModel>> GetBidPageAsync(string pageId);
		Task<IEnumerable<BidModel>> GetUserBidsAsync(string userId);
		Task<IEnumerable<BidHistoryModel>> GetBidHistoriesAsync(string userName);
		Task<GenericResponse> AddToBidHistoryAsync(BidHistoryRequest req);
		Task<GenericResponse> DeleteBidAsync(string bid_Id);
		Task<GenericResponse> UpdatePriceAsync(BidPriceUpdateRequest req);
		Task<string> ChangeStatusAsync(ChangeBidStatusRequest bidRequest);
		Task<IEnumerable<string>> GetBidImagesAsync(string bid_Id);

	}
}
