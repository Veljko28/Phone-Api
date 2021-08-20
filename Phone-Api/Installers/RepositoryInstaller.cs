﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phone_Api.Repository;
using Phone_Api.Repository.Interfaces;
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
			services.AddSingleton<IPhoneRepository, PhoneRepository>();
			services.AddSingleton<IBidRepository, BidRepository>();
			services.AddScoped<IUserRepository, UserRepository>();


			services.AddSingleton<IWishListRepository, WishListRepository>();
			services.AddSingleton<IPurchaseRepository, PurchaseRepository>();
			services.AddScoped<INotificationRepository, NotificationRepository>();


			services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
		}
	}
}
