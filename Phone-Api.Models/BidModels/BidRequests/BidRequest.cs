﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests.BidRequests
{
	public class BidRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public Decimal Price { get; set; }
		public string Category { get; set; }
		public string Brand { get; set; }
		public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
		public DateTime Date_Ends { get; set; } = DateTime.UtcNow;

	}
}
