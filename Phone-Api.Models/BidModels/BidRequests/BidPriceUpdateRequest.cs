using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests.BidRequests
{
	public class BidPriceUpdateRequest
	{
		public string Id { get; set; }
		public Decimal Price { get; set; }
	}
}
