using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models
{
	public class ContactModel
	{
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string Email { get; set; }

	}
}
