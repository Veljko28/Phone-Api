using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models
{
	public class RegisterUserModel
	{
		[Required]
		[MinLength(15), MaxLength(256)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(3), MaxLength(50)]
		public string UserName { get; set; }

		[Required]
		[MinLength(6), MaxLength(256)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[MinLength(6), MaxLength(256)]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

	}
}
