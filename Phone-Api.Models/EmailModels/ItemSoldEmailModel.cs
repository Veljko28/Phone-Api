using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.EmailModels
{
	public class ItemSoldEmailModel
	{
		public string Email { get; set; }
		public string BuyerId { get; set; }
		public string Type { get; set; }
		public string ItemName { get; set; }
	}
}
