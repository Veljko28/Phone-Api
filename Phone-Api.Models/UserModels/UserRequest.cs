using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class UserRequest
	{
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Confirm_Password { get; set; }

	}
}
