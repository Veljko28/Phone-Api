﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class WishListController : Controller
	{
		private readonly IWishListRepository _wishList;
		private readonly IPhoneRepository _phones;
		private readonly IBidRepository _bids;


		public WishListController(IWishListRepository wishList, IPhoneRepository phones, IBidRepository bids)
		{
			_wishList = wishList;
			_phones = phones;
			_bids = bids;
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
		public async Task<IActionResult> RemoveFromWishList([FromBody] WishListRemoveRequest model)
		{
			var result = await _wishList.RemoveFromWishListAsync(model.UserId, model.PhoneId);

			if (!result.Success)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("Removed the wish from the list");
		}

		[AllowAnonymous]
		[HttpPost(ApiRoutes.WishlistRoutes.GetUserWishes)]
		public async Task<IActionResult> GetUserWishes([FromBody] UserWishListRequest model)
		{
			var result = await _wishList.GetUserWishesAsync(model);

			if (!result.Any())
			{
				return BadRequest("Cannot find any wishes for this user");
			}


			if (model.Type == "phone")
			{
				List<PhoneModel> list = new List<PhoneModel>();

				foreach (var phoneId in result)
				{
					PhoneModel phone = await _phones.GetPhoneByIdAsync(phoneId);
					if (phone == null)
					{
						await _wishList.RemoveFromWishListAsync(model.UserId, phoneId);
					}
					else list.Add(phone);
				}

				return Ok(list);
			}

			else
			{
				List<BidModel> list = new List<BidModel>();

				foreach (var bid_Id in result)
				{
					BidModel bid = await _bids.GetBidByIdAsync(bid_Id);
					if (bid == null)
					{
						await _wishList.RemoveFromWishListAsync(model.UserId, bid_Id);
					}
					else list.Add(bid);
				}

				return Ok(list);
			}

		}

		[AllowAnonymous]
		[HttpPost(ApiRoutes.WishlistRoutes.IsInWishList)]
		public async Task<IActionResult> IsInWishList([FromBody] PageInWishListRequest model)
		{
			List<string> phoneIds = await _wishList.FindUserWishListAsync(model);

			return Ok(phoneIds);
		}

		[AllowAnonymous]
		[HttpGet(ApiRoutes.WishlistRoutes.GetPhoneFavorites)]
		public async Task<IActionResult> GetPhoneFavorites([FromRoute] string phoneId)
		{
			int phoneFavorites = await _wishList.GetPhoneFavoritesAsync(phoneId);

			return Ok(phoneFavorites);
		}
	}
}
