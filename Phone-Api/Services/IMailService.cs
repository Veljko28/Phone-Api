using Phone_Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Services
{
	public interface IMailService
	{
		Task SendCofirmEmailAsync(string email);
		Task SendItemSoldEmailAsync(ItemSoldEmailModel model);
	}
}
