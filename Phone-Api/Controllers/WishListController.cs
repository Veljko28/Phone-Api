using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.Requests;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class WishListController : Controller
	{
		private readonly IWishListRepository _wishList;

		public WishListController(IWishListRepository wishList)
		{
			_wishList = wishList;
		}

		[HttpPost(ApiRoutes.WishlistRoutes.AddToWishList)]
		public async Task<IActionResult> AddToWishList([FromBody] WishListRequest request)
		{
			var result = await _wishList.AddToWishListAsync(request);

			if (!result.Success)
			{
				BadRequest(result.ErrorMessage);
			}

			return Ok("Added to wish list");
		}


		[HttpDelete(ApiRoutes.WishlistRoutes.RemoveFromWishList)]
		public async Task<IActionResult> RemoveFromWishList([FromRoute] string wishId)
		{
			var result = await _wishList.RemoveFromWishListAsync(wishId);

			if (!result.Success)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("Removed the wish from the list");
		}


		[HttpGet(ApiRoutes.WishlistRoutes.GetUserWishes)]
		public async Task<IActionResult> GetUserWishes([FromRoute] string userId)
		{
			var result = await _wishList.GetUserWishesAsync(userId);

			if (!result.Any())
			{
				return BadRequest("Cannot find any wishes for this user");
			}

			return Ok(result);
		}


	}
}
