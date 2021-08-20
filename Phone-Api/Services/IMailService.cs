using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phone_Api.Services
{
	public interface IMailService
	{
		Task SendCofirmEmailAsync(string email);
	}
}
