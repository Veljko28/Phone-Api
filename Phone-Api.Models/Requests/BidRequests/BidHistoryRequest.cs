using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests.BidRequests
{
	public class BidHistoryRequest
	{
		public string Bid_Id { get; set; }
		public string UserName { get; set; }
		public Decimal Amount { get; set; }
	}
}
