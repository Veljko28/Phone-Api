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
	public class PhoneController : Controller
	{
		private readonly IPhoneRepository _phones;

		public PhoneController(IPhoneRepository phones)
		{
			_phones = phones;
		}

		[HttpPost(ApiRoutes.PhoneRoutes.Add)]
		public async Task<IActionResult> AddPhone([FromBody] PhoneRequest phoneRequest, [FromBody] string userId)
		{
			var phone = await _phones.addPhoneAsync(phoneRequest, userId);

			if (phone == null)
			{
				return BadRequest("Error while creating the phone");
			}

			return Ok(phone);
		}

		[HttpGet(ApiRoutes.PhoneRoutes.GetById)]
		public async Task<IActionResult> GetById([FromRoute] string phoneId)
		{
			return Ok("phone id = " + phoneId);

			var phone = await _phones.getPhoneByIdAsync(phoneId);

			if (phone == null)
			{
				return BadRequest("No phone was found with id " + phoneId);
			}

			return Ok(phone);
		}
	}
}
