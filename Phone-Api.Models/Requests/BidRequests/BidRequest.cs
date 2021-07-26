using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests.BidRequests
{
	public class BidRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Decimal Price { get; set; }
		public string Seller { get; set; }
		public string Category { get; set; }
		public string Brand { get; set; }
		public DateTime TimeCreated { get; set; }
		public DateTime TimeEnds { get; set; }

	}
}
