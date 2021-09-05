using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class ChangePasswordRequest
	{
		public string Current_Password { get; set; }
		public string Confirm_Current_Password { get; set; }
		public string New_Password { get; set; }
	}
}
