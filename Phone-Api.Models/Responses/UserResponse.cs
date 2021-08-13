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
		public string Contanct { get; set; }
	}
}
