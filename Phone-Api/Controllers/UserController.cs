using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phone_Api.Helpers;
using Phone_Api.Models.Requests;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpPost(ApiRoutes.Users.Register)]
		public async Task<IActionResult> Register([FromBody] UserModel model)
		{
			var user = new IdentityUser() { UserName = model.UserName, Email = model.Email };
			var result = await userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await signInManager.SignInAsync(user, isPersistent: false);
				return Ok();
			}

			return BadRequest(result.Errors.ToList());
		}


		[HttpPost(ApiRoutes.Users.Login)]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await userManager.FindByNameAsync(model.UserName);

			if (user.UserName == model.UserName)
			{
				await signInManager.SignInAsync(user, model.RememberMe);

				return Ok();
			}

			return BadRequest("Failed to Login");
		}
	}
}
