using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class EditModel
	{
		public PhoneModel Model { get; set; }
		public IEnumerable<string> Images { get; set; }
	}
}
