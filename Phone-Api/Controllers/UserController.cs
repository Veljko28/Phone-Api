﻿using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepository _users;

		public UserController(IUserRepository users)
		{
			_users = users;
		}

		[HttpPost(ApiRoutes.UserRoutes.Register)]
		public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
		{
			GenericResponse registered = await _users.RegisterAsync(userRequest);

			if (!registered.Success)
			{
				return BadRequest(registered.ErrorMessage);
			}

			return Ok("Successfully registered !");
		}

		[HttpPost(ApiRoutes.UserRoutes.Login)]
		public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
		{
			UserModel user = await _users.LoginAsync(loginRequest);

			if (user == null)
			{
				return BadRequest("Failed to Login");
			}

			return Ok(user);
		}

		[HttpPatch(ApiRoutes.UserRoutes.ChangePassword)]
		public async Task<IActionResult> ChangePassword([FromRoute] string userId, [FromBody] ChangePasswordRequest change)
		{
			GenericResponse response = await _users.ChangePasswordAsync(userId, change);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok("You have successfully changed your password");
		}
	}
}
