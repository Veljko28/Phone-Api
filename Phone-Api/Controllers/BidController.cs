using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.Requests.BidRequests;
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

				return genericResponse(new { bids, numOfPages }, "Failed to get latest phones");
			}

			return Ok(bids);
		}
		
		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.GetPage)]
		public async Task<IActionResult> GetPage([FromRoute] string pageId)
		{
			var phones = await _bids.GetBidPageAsync(pageId);
			if (pageId == "1")
			{
				int numOfPages = await _bids.GetNumOfPagesAsync();

				return genericResponse(new { phones, numOfPages }, "Failed to get latest phones");
			}

			return genericResponse(phones, "failed to get latest phones");
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
