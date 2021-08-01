using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
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

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("/hello")]
		public IActionResult Hello()
		{
			return Ok("hello");
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
			TokenResponse token = await _users.LoginAsync(loginRequest);

			if (token == null)
			{
				return BadRequest("Failed to Login");
			}

			return Ok(token);
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


		[HttpPatch(ApiRoutes.UserRoutes.Refresh)]

		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
		{
			var response = await _users.RefreshTokenAsync(request.Token, request.RefreshToken);

			if (response == null)
			{
				return BadRequest();
			}

			return Ok(response);
		}


		[HttpGet(ApiRoutes.UserRoutes.GetUserById)]

		public async Task<IActionResult> GetUserById([FromRoute] string userId)
		{
			var response = await _users.GetUserByIdAsync(userId);

			if (response == null)
			{
				return BadRequest();
			}

			UserResponse userResponse = new UserResponse
			{
				Id = response.Id,
				Email = response.Email,
				Image = response.Image,
				UserName = response.UserName,
				Description = response.Description,
				Contanct = response.Contanct,
				Phones_sold = response.Phones_sold,
			};

			return Ok(userResponse);
		}

	}
}
