﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class PhoneModel
	{
		public string Id { get; set; }
		public string Image{ get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Decimal Price { get; set; }
		public string Seller { get; set; }
		public string Category { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
		public string Brand { get; set; }
		public PhoneStatus Status { get; set; }

	}
}
