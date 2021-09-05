using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class CouponRequest
	{
		public string UserId { get; set; }
		public string Coupon { get; set; }
	}
}
