using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class WishListRequest
	{
		public string UserId { get; set; }
		public string PhoneId { get; set; }
		public string Type { get; set; }
	}
}
