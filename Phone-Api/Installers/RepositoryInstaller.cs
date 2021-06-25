using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phone_Api.Repository.Implementation;
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
			services.AddScoped<IPhoneRepository, PhoneRepository>();
		}
	}
}
