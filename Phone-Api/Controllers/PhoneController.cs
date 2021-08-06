﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

		private IActionResult genericResponse<T>(T success, string error)
		{
			if (success == null)
			{
				return BadRequest(error);
			}

			return Ok(success);
		}
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost(ApiRoutes.PhoneRoutes.Add)]
		public async Task<IActionResult> AddPhone([FromBody] PhoneRequest phoneRequest, [FromRoute] string userId)
		{
			var phone = await _phones.AddPhoneAsync(phoneRequest, userId);

		    return genericResponse(phone, "Error while creating the phone");
			
		}
		
		[HttpGet(ApiRoutes.PhoneRoutes.GetById)]
		public async Task<IActionResult> GetById([FromRoute] string phoneId)
		{

			var phone = await _phones.GetPhoneByIdAsync(phoneId);

		    return genericResponse(phone, "No phone was found with id " + phoneId);
		}

		[HttpGet(ApiRoutes.PhoneRoutes.GetSellerPhones)]
		public async Task<IActionResult> GetSellerPhones([FromRoute] string sellerId)
		{
			var phones = await _phones.GetSellerPhonesByIdAsync(sellerId);

			return genericResponse(phones,"Cannot find any phones for this user");

		}
		
		[HttpGet(ApiRoutes.PhoneRoutes.GetImages)]
		public async Task<IActionResult> GetImages([FromRoute] string phoneId)
		{
			var phones = await _phones.GetPhoneImagesAsync(phoneId);

			return genericResponse(phones, "Cannot find any images for this phone");

		}

		[HttpGet(ApiRoutes.PhoneRoutes.Featured)]
		public async Task<IActionResult> Featured()
		{
			var phones = await _phones.GetFeaturedPhonesAsync();

			return genericResponse(phones, "failed to get featured phones");

		}

		[HttpGet(ApiRoutes.PhoneRoutes.Latest)]
		public async Task<IActionResult> Latest()
		{
			var phones = await _phones.GetLastestPhonesAsync();

			return genericResponse(phones, "failed to get latest phones");

		}


		[HttpGet(ApiRoutes.PhoneRoutes.GetPage)]
		public async Task<IActionResult> GetPage([FromRoute] string pageId)
		{
			var phones = await _phones.GetPhonePageAsync(pageId);

			return genericResponse(phones, "failed to get latest phones");

		}


		[HttpDelete(ApiRoutes.PhoneRoutes.Delete)]
		public async Task<IActionResult> Delete([FromRoute] string pageId)
		{
			var phone = await _phones.DeletePhoneAsync(pageId);

			if (phone.Success)
			{
				return Ok();
			}

			return BadRequest(phone.ErrorMessage);
		}
	}
}
