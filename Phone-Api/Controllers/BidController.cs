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
	public class BidController : Controller
	{
		private readonly IBidRepository _bids;

		public BidController(IBidRepository bids)
		{
			_bids = bids;
		}

		[HttpPost(ApiRoutes.BidRoutes.AddBid)]
		public async Task<IActionResult> AddBid([FromBody] BidRequest req)
		{
			var result = await _bids.AddBidAsync(req);

			if (!result.Success) 
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("Your bid has been added");
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


		[HttpGet(ApiRoutes.BidRoutes.BidById)]
		public async Task<IActionResult> BidById([FromRoute] string bid_Id)
		{
			var result = await _bids.GetBidByIdAsync(bid_Id);

			if (result == null) return BadRequest("Cannot find bid with id :" + bid_Id);

			return Ok(result);
		}


	}
}
