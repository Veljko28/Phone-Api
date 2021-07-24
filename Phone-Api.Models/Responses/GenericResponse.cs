using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Responses
{
	public class GenericResponse
	{
		public bool Success { get; set; }

		public string ErrorMessage { get; set; }
	}
}
