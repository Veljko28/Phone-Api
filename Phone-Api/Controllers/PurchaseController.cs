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

		public PurchaseController(IPurchaseRepository purchases)
		{
			_purchases = purchases;
		}

		[HttpPost(ApiRoutes.PurchaseRoutes.AddPurchase)]
		public async Task<IActionResult> AddPurchase([FromBody] PurchaseRequest req)
		{
			var result = await _purchases.AddPurchaseAsync(req);

			if (result == null) return BadRequest("Failed to add purchase to database");

			return Ok(result);
		}

		[HttpGet(ApiRoutes.PurchaseRoutes.GetPurchasePhones)]
		public async Task<IActionResult> GetPurchasePhones([FromRoute] string purchaseId)
		{
			var results = await _purchases.GetPurchasePhonesAsync(purchaseId);
			
			if (!results.Any())
			{
				return BadRequest("Cannot find any phones for this purchase");
			}

			return Ok(results);
		}


		[HttpPost(ApiRoutes.PurchaseRoutes.AddPhoneToPurchase)]
		public async Task<IActionResult> AddPhoneToPurchase([FromRoute] string purchaseId, [FromRoute] string phoneId)
		{
			var result = await _purchases.AddPhoneToPurchaseAsync(purchaseId, phoneId);

			if (!result.Success)
			{
				return BadRequest(result.ErrorMessage);
			}

			return Ok("Successfully added purchase");
		}



	}
}
