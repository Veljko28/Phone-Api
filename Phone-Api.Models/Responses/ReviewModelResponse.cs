using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Responses
{
	public class ReviewModelResponse
	{
		public string Id { get; set; }
		public int Rating { get; set; }
		public string BuyerId { get; set; }
		public string SellerId { get; set; }
		public string PhoneId { get; set; }
		public DateTime DateCreated { get; set; }
		public string Message { get; set; }
		public string UserName { get; set; }
	}
}
