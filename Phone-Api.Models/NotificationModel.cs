using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Models
{
	public class NotificationModel
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
		public string Message { get; set; }

	}
}
