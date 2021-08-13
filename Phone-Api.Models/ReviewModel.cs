using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class ReviewModel
	{
		public string Id { get; set; }
		public int Rating { get; set; }
		public string Message { get; set; }
		public string UserId { get; set; }
		public string PhoneId { get; set; }

		public DateTime DateCreated { get; set; } = DateTime.Now;
	}
}
