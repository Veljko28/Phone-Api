using Phone_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models.Responses
{
	public class UserModel
	{
		[Required]
		public string Id { get; set; }

		[Required]
		public string Image { get; set; }

		[Required]
		[MinLength(15), MaxLength(256)]
		[EmailAddress]
		public string Email { get; set; }
		
		[Required]
		[MinLength(6), MaxLength(256)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
		[Required]
		[MinLength(3), MaxLength(50)]
		public string UserName { get; set; }
		
		[Required]
		public IEnumerable<PhoneRequest> Listings { get; set; }
		
		[Required]
		public IEnumerable<BidRequest> BidListings { get; set; }

		// Take the overall rating as Ratings.Aggregate((a,b) => a + b) / Ratings.Count
		[Required]
		public IEnumerable<RatingModel> Ratings { get; set; }

		[Required]
		public int SoldPhones { get; set; }

		[Required]
		public ContactModel Contact { get; set; }
	}
}
