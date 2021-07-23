using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.BidModels
{
	public class BidHistoryModel
	{
		public string Id { get; set; }
		public string Bid_Id { get; set; }
		public string UserName { get; set; }
		public Decimal Amount { get; set; }

	}
}
