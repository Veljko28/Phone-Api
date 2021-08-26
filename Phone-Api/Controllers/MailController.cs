using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.EmailModels;
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
	}
}
