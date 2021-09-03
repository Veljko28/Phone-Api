using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Responses
{
	public class UserResponse
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Description { get; set; }
		public int Phones_sold { get; set; }
		public string PhoneNumber { get; set; }
		public bool EmailConfirmed { get; set; }
		public int LoyalityPoints { get; set; }
		public float Rating { get; set; }
	}
}
