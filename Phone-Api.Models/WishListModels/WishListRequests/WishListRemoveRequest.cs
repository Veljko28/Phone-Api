﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models.Requests
{
	public class WishListRemoveRequest
	{
		public string UserId { get; set; }
		public string PhoneId { get; set; }
	}
}
