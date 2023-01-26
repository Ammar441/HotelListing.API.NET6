using HotelListing.API.Data.ConfigDefaultData;
using HotelListing.API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
	public class HotelListingDbContext : IdentityDbContext<ApiUser>
	{
		public HotelListingDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Country> Countries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new RoleConfigrationData());
			modelBuilder.ApplyConfiguration(new CountryConfigrationData());
			modelBuilder.ApplyConfiguration(new HotelConfigrationData());
		}
	}
}
