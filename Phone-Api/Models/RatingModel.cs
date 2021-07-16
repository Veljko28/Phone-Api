using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models
{
	public class RatingModel
	{
		[Required]
		[MinLength(1),MaxLength(5)]
		public int Stars { get; set; }

		[Required]
		public UserModel Reviewer { get; set; }

		[Required]
		public string PhoneName { get; set; }
		
		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	}
}
