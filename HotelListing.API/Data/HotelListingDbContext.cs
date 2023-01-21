using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Sudan",
                    ShortName = "SD"
                }, new Country
                {
                    Id = 2,
                    Name = "Saudi Arabia",
                    ShortName = "KSA"
                },
                new Country
                {
                    Id = 3,
                    Name = "United Arab Emirates",
                    ShortName = "UAE"
                });
            modelBuilder.Entity<Hotel>().HasData(new Hotel
            {
                Id = 1,
                Name = "Hotel 1",
                CountryId = 1,
                Address = "Khartoum",
                Rating = 3
            },
            new Hotel
            {
                Id = 2,
                Name = "Hotel 2",
                CountryId = 1,
                Address = "Khartoum",
                Rating = 4
            },
            new Hotel
            {
                Id = 3,
                Name = "Hotel 3",
                CountryId = 2,
                Address = "Mecca",
                Rating = 5
            },
            new Hotel
            {
                Id = 4,
                Name = "Hotel 4",
                CountryId = 2,
                Address = "Mecca",
                Rating = 7
            });
        }
    }
}
