using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phone_Api.Models.Responses
{
	public class TokenResponse
	{
		[Required]
		public string Token { get; set; }

		[Required]
		public string RefreshToken { get; set; }

		[Required]
		public string Expires { get; set; }
	}
}
