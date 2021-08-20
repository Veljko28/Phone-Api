using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class ChangePhoneStatusRequest
	{
		public string PhoneId { get; set; }
		public PhoneStatus Status { get; set; }
	}
}
