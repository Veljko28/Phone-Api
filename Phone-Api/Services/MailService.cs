using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Phone_Api.Models.EmailModels;
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

		public async Task GenericEmail(string email, string subject, string link, string title, string text, string buttonText, string smallText)
		{
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
				message.To.Add(new MailboxAddress(email, email));
				message.Subject = subject;
				message.Body = new TextPart("html")
				{
					Text =
			  "<div style=\"text-align: center;background-color: #fff;padding: 20px\">" +
			   "<img style=\"width: 200px; text-align: center\"" +
			   "src=\"https://drive.google.com/thumbnail?id=1Q9PJsplffr9Bc8-WCWQcTc8NcHAzGhV3\" />" +
			  "<h1 style=\"color: #0cafe5\">" + title +"</h2>" +
			  "<h5 style=\"color: #999; text-align: center;margin-bottom: 45px\">" + text + "</h5>" +
			  "<a href=\"" + link + "\" style=\"padding: 20px;font-size:15px;border:none;color: #fff;border-radius:5px;background-color: #0cafe5;text-decoration: none\">" +
			   buttonText + "</a>" +
			"<p style=\"color: #999; font-size: 10px; margin-top: 45px\">" + smallText + "</p>" +
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

		public async Task SendCofirmEmailAsync(ConfirmEmailModel model)
		{
			await GenericEmail(model.Email,
				"Confirm Email - MobiStore - Online Mobile Store",
				"http://localhost:3000/confirmemail/" + model.ConfirmEmailId,
				"Email Confirmation",
				"Yay! You've created a MobiStore account with this email.<br/> Please take a moment " +
			    "to confirm that we can use this email address to send you emails ! ",
				 "Confirm your email address",
				 "A warm welcome by the MobiStore Support Team © 2021");
		}

		public async Task<string> SendCouponEmailAsync(string email, string amount)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			char[] stringChars = new char[15];
			Random random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			string coupon = new String(stringChars);

			await GenericEmail(email,
				"Your " + amount + " Off Coupon - MobiStore - Online Mobile Store",
				"http://localhost:3000",
				"Coupon " + amount + " Off",
				"You have successfully puchased the " + amount + " off coupon. Now go and use it ! <br/> <strong>Coupon: " + coupon +"</strong>",
				 "Go to website",
				 "MobiStore Support Team © 2021");

			return coupon;
		}

		public async Task SendItemSoldEmailAsync(ItemSoldEmailModel model)
		{
			await GenericEmail(model.Email,
				model.Name + " Has Been Sold - MobiStore - Online Mobile Store",
				"http://localhost:3000/user/" + model.BuyerId,
				 model.Type == "phone" ? model.Name + " Has Been Sold" : "The Bid For " + model.Name + " Has Finished !",
				 "Your " + model.Type + " has successfully " + (model.Type == "phone" ? "been sold" : "finished") + " ! Please contanct the buyer <br/> with the button below about the shipping or meet up " +
				 "for giving the phone.",
				 "Contact the Buyer",
				 "MobiStore Support Team © 2021"); ;
		}
	}
}
