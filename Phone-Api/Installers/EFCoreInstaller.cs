﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phone_Api.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phone_Api.Installers
{
	public class EFCoreInstaller : IInstaller
	{
		public void InstallServices(IConfiguration configuration, IServiceCollection services)
		{
			services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EmployeeDBConnection")));
		}
	}
}
