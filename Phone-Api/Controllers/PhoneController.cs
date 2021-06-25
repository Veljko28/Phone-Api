using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class PhoneController : Controller
	{
		private readonly IPhoneRepository _phoneRepository;
		public PhoneController(IPhoneRepository phoneRepository)
		{
			_phoneRepository = phoneRepository;
		}

		[HttpPost(ApiRoutes.Phones.Add)]
		public async Task<IActionResult> AddPhone([FromBody] PhoneRequest phone)
		{
			bool succeded = await _phoneRepository.AddPhoneAsync(phone);

			if (!succeded)
			{
				return BadRequest("Error while trying to add your phone");
			}

			return Ok(phone);
		}

		[HttpGet(ApiRoutes.Phones.Id)]
		public async Task<IActionResult> PhoneById([FromRoute] string phoneId)
		{
			PhoneResponse phone = await _phoneRepository.GetPhoneInfoByIdAsync(phoneId);

			if (phone == null)
			{
				return BadRequest("Cannot find the phone with Id: " + phoneId);

			}
			else return Ok(phone);

		}

		[HttpGet(ApiRoutes.Phones.Search)]
		public IActionResult FindPhones([FromRoute] string keyword)
		{
			var result = _phoneRepository.SearchPhonesAsync(keyword);

			if (result == null)
			{
				return BadRequest("No phones found");
			}

			return Ok(result);
		}

	}
}
