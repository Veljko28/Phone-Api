using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phone_Api.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phone_Api.Repository.Services
{
	public class AppDbContext : DbContext
	{
		private readonly IConfiguration configuration;

		public AppDbContext(IConfiguration configuration) : base()
		{
			this.configuration = configuration;
		}

		public DbSet<PhoneResponse> Phones { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
		}
	}
}
