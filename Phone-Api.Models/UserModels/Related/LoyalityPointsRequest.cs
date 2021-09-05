using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class LoyalityPointsRequest
	{
		public string UserId { get; set; }
		public int Amount { get; set; }
	}
}
