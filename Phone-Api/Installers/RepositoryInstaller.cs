using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phone_Api.Repository;
using Phone_Api.Repository.Helpers;
using Phone_Api.Repository.Interfaces;
using Phone_Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Installers
{
	public class RepositoryInstaller : IInstaller
	{
		public void InstallServices(IConfiguration configuration, IServiceCollection services)
		{
			var SmtpSettings = new SmtpSettings();
			configuration.Bind(nameof(SmtpSettings), SmtpSettings);
			services.AddSingleton(SmtpSettings);

			//services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

			services.AddSingleton<IPhoneRepository, PhoneRepository>();
			services.AddSingleton<IBidRepository, BidRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddSingleton<IMailService, MailService>();


			services.AddSingleton<IWishListRepository, WishListRepository>();
			services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
			services.AddScoped<INotificationRepository, NotificationRepository>();


			services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
		}
	}
}
