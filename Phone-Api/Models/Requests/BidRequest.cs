using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models.Requests
{
	public class BidRequest
	{
		[Required]
		public string Id { get; set; }

		[Required]
		[MinLength(5), MaxLength(100)]
		public string Name { get; set; }
		[Required]
		[MinLength(10), MaxLength(256)]
		public string Description { get; set; }
		[Required]
		public Decimal Price { get; set; }
		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
		[Required]
		public DateTime Exires { get; set; }
		[Required]
		public UserModel Seller { get; set; }
		[Required]
		public IEnumerable<BidModel> BidHistory { get; set; }
	}
}
