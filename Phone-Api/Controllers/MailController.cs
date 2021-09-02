using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.EmailModels;
using Phone_Api.Models.Requests;
using Phone_Api.Repository.Interfaces;
using Phone_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class MailController : Controller
	{
		private readonly IMailService _mail;
		private readonly IUserRepository _users;

		public MailController(IMailService mail, IUserRepository users)
		{
			_mail = mail;
			_users = users;
		}

		[HttpPost(ApiRoutes.EmailRoutes.ConfirmEmail)]
		public async Task<IActionResult> SendEmail([FromBody] ConfirmEmailModel model)
		{
			var user = await _users.GetUserByEmailAsync(model.Email);

			if (user.EmailConfirmed)
			{
				return BadRequest("The email has already been confirmed");
			}

			await _mail.SendCofirmEmailAsync(model);

			return Ok();
		}

		[HttpPost(ApiRoutes.EmailRoutes.ItemSold)]
		public async Task<IActionResult> ItemSold([FromBody] ItemSoldEmailModel model)
		{
			await _mail.SendItemSoldEmailAsync(model);

			bool sold = await _users.UpdatePhonesSoldAsync(model.SellerId);

			if (!sold)
			{
				return BadRequest("Failed to sell the phone");
			}

			return Ok();
		}

		[HttpPost(ApiRoutes.EmailRoutes.SendCoupon)]
		public async Task<IActionResult> SendCoupon([FromBody] LoyalityPointsRequest request)
		{
			string email = await _users.GetEmailByIdAsync(request.UserId);
			string amount = request.Amount.ToString() + "%";

			string coupon = await _mail.SendCouponEmailAsync(email, amount);

			if (coupon == null)
			{
				return BadRequest("Failed to send the coupon");
			}

			CouponModel model = new CouponModel
			{
				UserId = request.UserId,
				Amount = amount,
				Coupon = coupon
			};

			var response = await _users.AddUserCouponAsync(model);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}
	}
}
