using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class PageInWishListRequest
	{
		public string UserId { get; set; }
		public PhoneModel[] List { get; set; }
		public string Type { get; set; }
	}
}
