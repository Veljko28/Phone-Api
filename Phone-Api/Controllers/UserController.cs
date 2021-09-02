using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using Phone_Api.Repository.Interfaces;
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

			string userId = await _users.RegisterAsync(userRequest);

			if (userId == null)
			{
				return BadRequest("Failed to register");
			}

			return Ok(new { Id = userId });
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


		[HttpPost(ApiRoutes.UserRoutes.Refresh)]

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
				UserName = response.UserName,
				Description = response.Description,
				PhoneNumber = response.PhoneNumber,
				Phones_sold = response.Phones_sold,
				EmailConfirmed = response.EmailConfirmed,
				Rating = response.Rating
			};

			return Ok(userResponse);
		}

		[HttpGet(ApiRoutes.UserRoutes.GetUserNameById)]

		public async Task<IActionResult> GetUserNameById([FromRoute] string userId)
		{
			var response = await _users.GetUserNameByIdAsync(userId);
			
			if (response == null)
			{
				return NotFound("Failed to find user with id " + userId);
			}

			return Ok(response);
		}

		[HttpPost(ApiRoutes.UserRoutes.EditUserProfile)]

		public async Task<IActionResult> EditUserProfile([FromRoute] string userId, [FromBody] EditProfileModel model)
		{
			var response = await _users.EditUserProfileAsync(userId, model);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

		[HttpGet(ApiRoutes.UserRoutes.GetUserEmail)]
		public async Task<IActionResult> GetUserEmail([FromRoute] string userId)
		{
			string email = await _users.GetEmailByIdAsync(userId);

			if (email == null)
			{
				return BadRequest("Failed to get the email");
			}

			return Ok(email);
		}

		[HttpGet(ApiRoutes.UserRoutes.GetUserIdByName)]
		public async Task<IActionResult> GetUserIdByName([FromRoute] string userName)
		{
			string email = await _users.GetUserIdByNameAsync(userName);

			if (email == null)
			{
				return BadRequest("Failed to get the email");
			}

			return Ok(email);
		}

	

		[HttpPost(ApiRoutes.UserRoutes.AddLoyalityPoints)]
		public async Task<IActionResult> AddLoyalityPoints([FromBody] LoyalityPointsRequest request )
		{
			var response = await _users.AddLoyalityPointsAsync(request);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

		[HttpGet(ApiRoutes.UserRoutes.GetLoyalityPoints)]
		public async Task<IActionResult> GetLoyalityPoints([FromRoute] string userId)
		{
			int points = await _users.GetLoyalityPointsAsync(userId);

			return Ok(points);
		}

		[HttpPost(ApiRoutes.UserRoutes.RemoveLoyalityPoints)]
		public async Task<IActionResult> RemoveLoyalityPoints([FromBody] LoyalityPointsRequest request)
		{
			var response = await _users.RemoveLoyalityPointsAsync(request);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

		[HttpGet(ApiRoutes.UserRoutes.GetUserReviews)]
		public async Task<IActionResult> GetUserReviews([FromRoute] string userId)
		{
			var list = await _users.GetUserReviewsAsync(userId);
			List<ReviewModelResponse> responses = new List<ReviewModelResponse>();

			foreach (var item in list)
			{
				string Username = await _users.GetUserNameByIdAsync(item.BuyerId);

				ReviewModelResponse model = new ReviewModelResponse
				{
					Id = item.Id,
					BuyerId = item.BuyerId,
					SellerId = item.SellerId,
					PhoneId = item.PhoneId,
					DateCreated = item.DateCreated,
					Message = item.Message,
					Rating = item.Rating,
					UserName = Username
				};

				responses.Add(model);

			}

			return Ok(responses);
		}

		[HttpPatch(ApiRoutes.UserRoutes.UpdateRating)]
		public async Task<IActionResult> UpdateRating([FromRoute] string userId)
		{
			var reviews = await _users.GetUserReviewsAsync(userId);

			double rating = 0;

			foreach (var review in reviews)
			{
				rating += review.Rating;
			}

			rating /= reviews.Count();

			var response = await _users.UpdateRatingAsync(rating, userId);

			if (!response.Success)
			{
				return BadRequest(response.ErrorMessage);
			}

			return Ok();
		}

	}
}
