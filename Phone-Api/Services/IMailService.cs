using Phone_Api.Models.EmailModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Services
{
	public interface IMailService
	{
		Task SendCofirmEmailAsync(ConfirmEmailModel model);
		Task SendItemSoldEmailAsync(ItemSoldEmailModel model);
		Task<string> SendCouponEmailAsync(string email, string amount);
		Task SendForgotPasswordEmailAsync(string email, string newPassword);
	}
}
