using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class UserWishListRequest
	{
		public string UserId { get; set; }
		public string Type { get; set; }
	}
}
