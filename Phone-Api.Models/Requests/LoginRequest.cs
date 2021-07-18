using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class LoginRequest
	{
		public string Email_UserName { get; set; }

		public string Password { get; set; }
	}
}
