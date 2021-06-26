using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Models.Requests
{
	public class LoginModel
	{
		[Required]
		[MinLength(3), MaxLength(50)]
		public string UserName { get; set; }
		[Required]
		[MinLength(6), MaxLength(256)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; } = false;
	}
}
