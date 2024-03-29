﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class PhoneController : Controller
	{
		private readonly IPhoneRepository _phones;
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _environment;

		public PhoneController(IPhoneRepository phones, IConfiguration configuration, IWebHostEnvironment environment)
		{
			_phones = phones;
			_configuration = configuration;
			_environment = environment;
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

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost(ApiRoutes.PhoneRoutes.ChangeStatus)]
		public async Task<IActionResult> ChangeStatus([FromBody] ChangePhoneStatusRequest phoneRequest)
		{
			GenericResponse response = await _phones.ChangeStatusAsync(phoneRequest);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

		[HttpGet(ApiRoutes.PhoneRoutes.GetById)]
		public async Task<IActionResult> GetById([FromRoute] string phoneId)
		{

			var phone = await _phones.GetPhoneByIdAsync(phoneId);

		    return genericResponse(phone, "No phone was found with id " + phoneId);
		}

		[HttpGet(ApiRoutes.PhoneRoutes.GetSellerPhones)]
		public async Task<IActionResult> GetSellerPhones([FromRoute] string sellerId, [FromRoute] int pageNum)
		{
			var phones = await _phones.GetSellerPhonesByIdAsync(sellerId, pageNum);

			if (pageNum == 1)
			{
				int numOfPages = await _phones.GetNumOfPagesAsync(sellerId);

				return genericResponse(new { phones, numOfPages }, "Failed to get latest phones");
			}

			return genericResponse(phones,"Cannot find any phones for this user");

		}
		
		[HttpGet(ApiRoutes.PhoneRoutes.GetImages)]
		public async Task<IActionResult> GetImages([FromRoute] string phoneId)
		{
			var phones = await _phones.GetPhoneImagesAsync(phoneId);

			return genericResponse(phones, "Cannot find any images for this phone");

		}

		[HttpGet(ApiRoutes.PhoneRoutes.Featured)]
		public async Task<IActionResult> Featured([FromRoute] string phoneId)
		{
			var phones = await _phones.GetFeaturedPhonesAsync(phoneId);

			return genericResponse(phones, "failed to get featured phones");

		}

		[HttpGet(ApiRoutes.PhoneRoutes.Latest)]
		public async Task<IActionResult> Latest()
		{
			var phones = await _phones.GetLastestPhonesAsync();

			return genericResponse(phones, "failed to get latest phones");

		}


		[HttpGet(ApiRoutes.PhoneRoutes.GetPhones)]
		public async Task<IActionResult> GetPhones()
		{
			var phones = await _phones.GetPhonesAsync();

			return genericResponse(phones, "Failed to get phones");

		}

		[HttpDelete(ApiRoutes.PhoneRoutes.Delete)]
		public async Task<IActionResult> Delete([FromRoute] string phoneId)
		{
			var phone = await _phones.DeletePhoneAsync(phoneId);

			if (phone.Success)
			{
				return Ok();
			}

			return BadRequest(phone.ErrorMessage);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPatch(ApiRoutes.PhoneRoutes.Edit)]
		public async Task<IActionResult> Edit([FromBody] EditModel editModel)
		{
			var phone = await _phones.EditPhoneAsync(editModel.Model);

			List<string> Images = (await _phones.GetPhoneImagesAsync(editModel.Model.Id)).ToList();

			foreach (string image in Images)
			{
				if (!editModel.Images.Contains(image))
				{
					string sql = "exec [_spRemovePhoneImage] @ImagePath, @PhoneId";

					string imageName = image.Split('/').LastOrDefault();
					string fullPath = Path.Combine(_environment.WebRootPath + "\\Uploads\\" + imageName);

					bool removed = (await DatabaseOperations.GenericExecute(sql, new { ImagePath = image, PhoneId = editModel.Model.Id }, _configuration, "Failed to remove the image")).Success;

					try
					{
						if (System.IO.File.Exists(fullPath))
						{
							System.IO.File.Delete(fullPath);
						}

					}
					catch (Exception err)
					{
						throw new Exception(err.Message);
					}

					if (!removed)
					{
						return BadRequest("Failed to remove one of the images");
					}

				}
			}

			if (phone.Success)
			{
				return Ok();
			}

			return BadRequest(phone.ErrorMessage);
		}


		[HttpGet(ApiRoutes.PhoneRoutes.UserNumberOfPhonesSelling)]
		public async Task<IActionResult> UserNumberOfPhonesSelling([FromRoute] string userId)
		{
			int numOfPhones = await _phones.GetNumOfUserPhonesAsync(userId);

			return Ok(numOfPhones);
		}
	}
}
