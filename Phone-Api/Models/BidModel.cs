using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models
{
	public class BidModel
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public Decimal BidAmount { get; set; }

	}
}
