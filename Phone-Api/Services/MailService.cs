using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Services
{
	public class MailService : IMailService
	{
		private readonly SmtpSettings _settings;
		private readonly IWebHostEnvironment _env;
		public MailService(SmtpSettings settings, IWebHostEnvironment env)
		{
			_settings = settings;
			_env = env;
		}

		public async Task SendCofirmEmailAsync(string email)
		{
			try
			{
				string id = Guid.NewGuid().ToString();
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
				message.To.Add(new MailboxAddress(email, email));
				message.Subject = "Confirm Email - MobiStore - Online Mobile Store";
				message.Body = new TextPart("html")
				{
					Text = 
			  "<div style=\"text-align: center;background-color: #fff;padding: 20px\">" +
			   "<img style=\"width: 200px; text-align: center\"" +
			   "src=\"https://drive.google.com/thumbnail?id=1Q9PJsplffr9Bc8-WCWQcTc8NcHAzGhV3\" />" +
			  "<h2 style=\"color: #0cafe5\">Email Confirmation</h2>" +
			  "<p style=\"color: #999; text-align: center;margin-bottom: 45px\">Yay ! You've created a MobiStore account with this email.<br/> Please take a moment " +
			  "to confirm that we can use this email address to send you emails !</p>" +
			  "<a href=\"http://localhost:3000/confirmemail/" + id +"\" style=\"padding: 20px;font-size:15px;border:none;color: #fff;border-radius:5px;background-color: #0cafe5;text-decoration: none\">" +
			  "Confirm your email address</a>" +
			"<p style=\"color: #999; font-size: 10px; margin-top: 45px\">A warm welcome by the MobiStore Support Team</p>" +
		"</div>"
	 };
				

				using (SmtpClient client = new SmtpClient())
				{
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					if (_env.IsDevelopment())
					{
						await client.ConnectAsync(_settings.Server, _settings.Port, true);
					}
					else
					{
						await client.ConnectAsync(_settings.Server);
					}

					await client.AuthenticateAsync(_settings.Username, _settings.Password);
					await client.SendAsync(message);
					await client.DisconnectAsync(true);

				}

			}
			catch (Exception e)
			{
				throw new InvalidOperationException(e.Message);
			}
		}
	}
}
