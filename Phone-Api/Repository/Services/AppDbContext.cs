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
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<PhoneResponse> Phones { get; set; }
	}
}
