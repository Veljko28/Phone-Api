using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Requests.BidRequests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BidController : Controller
	{
		private readonly IBidRepository _bids;

		public BidController(IBidRepository bids)
		{
			_bids = bids;
		}

		private IActionResult genericResponse<T>(T success, string error)
		{
			if (success == null)
			{
				return BadRequest(error);
			}

			return Ok(success);
		}

		[HttpPost(ApiRoutes.BidRoutes.AddBid)]
		public async Task<IActionResult> AddBid([FromBody] BidRequest req, [FromRoute] string userId)
		{
			var result = await _bids.AddBidAsync(req, userId);

			return genericResponse(result, "Error while creating the bid");
		}

		[HttpPost(ApiRoutes.BidRoutes.AddHistory)]
		public async Task<IActionResult> AddHistory([FromBody] BidHistoryRequest req)
		{
			var bid = await _bids.GetBidByIdAsync(req.Bid_Id);

			if (bid.Date_Ends < DateTime.UtcNow)
			{
				return BadRequest("The bid has already ended");
			}


			var result = await _bids.AddToBidHistoryAsync(req);

			if (!result.Success)
			{
				return BadRequest(result.ErrorMessage);
			}

			var phone = await _bids.UpdatePriceAsync(new BidPriceUpdateRequest {Id = req.Bid_Id, Price = req.Amount });

			if (!phone.Success)
			{
				return BadRequest(phone.ErrorMessage);
			}

			return Ok(true);
		}

		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.BidHistories)]
		public async Task<IActionResult> BidHistories([FromRoute] string bid_Id)
		{
			var results = await _bids.GetBidHistoriesAsync(bid_Id);

			return Ok(results);
		}

		[HttpGet(ApiRoutes.BidRoutes.GetPlacedBids)]
		public async Task<IActionResult> GetPlacedBids([FromRoute] string userName, [FromRoute] int page)
		{
			List<BidModel> phones = await _bids.GetPlacedBidsAsync(userName, page);

			int numOfPages = await _bids.GetPlacedBidsNumOfPagesAsync(userName);

			return Ok(new { phones, numOfPages });
		}

		[HttpPost(ApiRoutes.BidRoutes.ChangeStatus)]
		public async Task<IActionResult> ChangeStatus([FromBody] ChangeBidStatusRequest bidRequest)
		{
			string userName = await _bids.ChangeStatusAsync(bidRequest);

			if (userName == null)
			{
				return BadRequest();
			}

			return Ok(new { userName });
		}


		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.BidById)]
		public async Task<IActionResult> BidById([FromRoute] string bid_Id)
		{
			var result = await _bids.GetBidByIdAsync(bid_Id);

			if (result == null) return BadRequest("Cannot find bid with id :" + bid_Id);

			return Ok(result);
		}


		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.UserBids)]
		public async Task<IActionResult> UserBids([FromRoute] string userId, [FromRoute] int pageNum)
		{
			var bids = await _bids.GetUserBidsAsync(userId);

			if (bids == null) return BadRequest("Cannot find any bids for user with id :" + userId);

			if (pageNum == 1)
			{
				int numOfPages = await _bids.GetNumOfPagesAsync();

				return genericResponse(new { bids, numOfPages }, "Failed to get user bids");
			}

			return Ok(bids);
		}
		
		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.GetBids)]
		public async Task<IActionResult> GetBids()
		{
			var bids = await _bids.GetBidsAsync();
			
			return genericResponse(bids, "failed to get all bids");
		}

		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.GetImages)]
		public async Task<IActionResult> GetImages([FromRoute] string bid_Id)
		{
			var phones = await _bids.GetBidImagesAsync(bid_Id);

			return genericResponse(phones, "Cannot find any images for this phone");

		}

		[HttpDelete(ApiRoutes.BidRoutes.Delete)]
		public async Task<IActionResult> Delete([FromRoute] string bid_Id)
		{
			var phone = await _bids.DeleteBidAsync(bid_Id);

			if (phone.Success)
			{
				return Ok();
			}

			return BadRequest(phone.ErrorMessage);
		}

	}
}
