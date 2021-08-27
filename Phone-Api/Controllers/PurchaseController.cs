using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class PurchaseController : Controller
	{
		private readonly IPurchaseRepository _purchases;
		private readonly IPhoneRepository _phones;

		public PurchaseController(IPurchaseRepository purchases, IPhoneRepository phones)
		{
			_purchases = purchases;
			_phones = phones;
		}

		[HttpPost(ApiRoutes.PurchaseRoutes.AddPurchase)]
		public async Task<IActionResult> AddPurchase([FromBody] IEnumerable<PurchaseRequest> req)
		{
			var result = await _purchases.AddPurchaseAsync(req);

			if (!result.Success) return BadRequest(result.ErrorMessage);

			return Ok();
		}

		[HttpGet(ApiRoutes.PurchaseRoutes.GetPurchasedPhones)]
		public async Task<IActionResult> GetPurchasedPhones([FromRoute] string userId, [FromRoute] int page)
		{
			IEnumerable<string> phoneIds = await _purchases.GetPurchasedPhonesPageAsync(userId, page);

			List<PhoneModel> phones = new List<PhoneModel>();

			foreach (string phoneId in phoneIds)
			{
				PhoneModel phone = await _phones.GetPhoneByIdAsync(phoneId);

				if (phone != null) phones.Add(phone);
			}
			if (page == 1)
			{
				int numOfPages = await _purchases.GetNumOfPagesAsync(userId);

				return Ok(new { phones, numOfPages });

			}

			return Ok(phones);
		}

		[HttpGet(ApiRoutes.PurchaseRoutes.PhoneBoughtByUser)]
		public async Task<IActionResult> PhoneBoughtByUser([FromRoute] string userId, [FromRoute] string phoneId)
		{
			var result = await _purchases.PhoneBoughtByUserAsync(userId, phoneId);

			if (!result.Success)
			{
				return NotFound(result.ErrorMessage);
			}

			return Ok(true);
		}
	}
}
