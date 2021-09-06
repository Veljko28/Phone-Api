using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class PurchaseRequest
	{
		public string BuyerId { get; set; }
		public string SellerId { get; set; }
		public string PhoneId { get; set; }

	}
}
