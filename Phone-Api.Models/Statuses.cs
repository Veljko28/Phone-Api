using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public enum PhoneStatus
	{
		Running,
		Sold,
		Deleted
	}

	public enum BidStatus
	{
		Running,
		Won,
		Lost,
		Failed,
		Deleted
	}
}
