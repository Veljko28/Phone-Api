using Microsoft.AspNetCore.Mvc;
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
		private readonly IPhoneRepository _phoneRepository;
		public PhoneController(IPhoneRepository phoneRepository)
		{
			_phoneRepository = phoneRepository;
		}

		[HttpGet]
		public async Task<IActionResult> AddPhone([FromBody] PhoneRequest phone)
		{
			bool succeded = await _phoneRepository.AddPhoneAsync(phone);

			if (!succeded)
			{
				return BadRequest("Error while trying to add your phone");
			}

			return Ok(phone);
		}
	}
}
