using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Services
{
	public class SmtpSettings
	{
		public string Server { get; set; }
		public int Port { get; set; }
		public string SenderName { get; set; }
		public string SenderEmail { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

	}
}
