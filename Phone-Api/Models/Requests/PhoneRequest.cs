using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class PhoneRequest
	{
		[Required]
		public string Id { get; set; }

		[Required]
		public string Image { get; set; }

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
		public UserModel Seller { get; set; }

		[Required]
		public CategoryModel Category { get; set; }

		[Required]
		public BrandModel Brand { get; set; }

	}
}
