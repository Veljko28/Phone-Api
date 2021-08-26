using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests.BidRequests
{
	public class ChangeBidStatusRequest
	{
		public string Bid_Id { get; set; }
		public BidStatus Status { get; set; }
	}
}
