using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class SearchModel
	{
		public IEnumerable<PhoneModel> Phones { get; set; }
		public IEnumerable<BidModel> Bids { get; set; }
		public IEnumerable<UserModel> Users { get; set; }

	}
}
