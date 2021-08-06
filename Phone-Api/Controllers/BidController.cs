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

			return genericResponse(result, "Error while creating the phone");
		}

		[HttpPost(ApiRoutes.BidRoutes.AddHistory)]
		public async Task<IActionResult> AddHistory([FromBody] BidHistoryRequest req)
		{
			var result = await _bids.AddToBidHistoryAsync(req);

			if (!result.Success)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok(result.Success);
		}

		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.BidHistories)]
		public async Task<IActionResult> BidHistories([FromRoute] string bid_Id)
		{
			var results = await _bids.GetBidHistoriesAsync(bid_Id);

			if (!results.Any())
			{
				return BadRequest("The history is empty");
			}

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
		public async Task<IActionResult> UserBids([FromRoute] string userId)
		{
			var result = await _bids.GetUserBidsAsync(userId);

			if (result == null) return BadRequest("Cannot find any bids for user with id :" + userId);

			return Ok(result);
		}
		
		[AllowAnonymous]
		[HttpGet(ApiRoutes.BidRoutes.GetPage)]
		public async Task<IActionResult> GetPage([FromRoute] string pageId)
		{
			var phones = await _bids.GetBidPageAsync(pageId);

			if (phones == null || phones.Count() == 0)
			{
				return BadRequest("Failed to find any bids");
			}

			return Ok(phones);
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
