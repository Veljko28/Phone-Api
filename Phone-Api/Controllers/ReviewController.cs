using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.ReviewModels.Requests;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class ReviewController : Controller
	{
		private readonly IReviewRepository _reviews;

		public ReviewController(IReviewRepository reviews)
		{
			_reviews = reviews;
		}

		[HttpPost(ApiRoutes.ReviewRoutes.AddReview)]
		public async Task<IActionResult> AddReview([FromBody] ReviewModelRequest req)
		{
			var response = await _reviews.AddReviewAsync(req);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

		[HttpGet(ApiRoutes.ReviewRoutes.Reviewed)]
		public async Task<IActionResult> Reviewed([FromRoute] string buyerId, [FromRoute] string phoneId)
		{
			ReviewedCheckRequest request = new ReviewedCheckRequest
			{
				BuyerId = buyerId,
				PhoneId = phoneId
			};

			bool reviewed = await _reviews.CheckReviewedAsync(request);

			if (reviewed)
			{
				return BadRequest("This user has already reviewed this phone");
			}

			return Ok();
		}
	}
}
