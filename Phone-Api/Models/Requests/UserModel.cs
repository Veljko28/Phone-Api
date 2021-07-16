using Microsoft.AspNetCore.Identity;
using Phone_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models.Responses
{
	public class UserModel : IdentityUser
	{
		[Required]
		public string Image { get; set; }

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
