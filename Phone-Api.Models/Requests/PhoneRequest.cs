﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class PhoneRequest
	{
		public string Image { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string Brand { get; set; }
	}
}