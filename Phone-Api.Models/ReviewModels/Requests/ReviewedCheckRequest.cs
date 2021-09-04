using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.ReviewModels.Requests
{
	public class ReviewedCheckRequest
	{
		public string BuyerId { get; set; }
		public string PhoneId { get; set; }
	}
}
