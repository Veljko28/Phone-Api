using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class PurchaseRequest
	{
		public string UserId { get; set; }
		public Decimal Total { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
