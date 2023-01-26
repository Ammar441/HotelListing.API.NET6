using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.ConfigDefaultData
{
	public class CountryConfigrationData : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.HasData(
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

		}
	}
}
