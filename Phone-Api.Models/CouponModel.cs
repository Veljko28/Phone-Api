using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class CouponModel
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string Coupon { get; set; }
		public string Amount { get; set; }
		public bool Valid { get; set; }
	}
}
