using Microsoft.AspNetCore.Mvc;
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

		public MailController(IMailService mail)
		{
			_mail = mail;
		}

		[HttpPost("/email")]
		public async Task<IActionResult> SendEmail()
		{
			await _mail.SendCofirmEmailAsync("charlibear284@gmail.com");

			return Ok();
		}
	}
}
