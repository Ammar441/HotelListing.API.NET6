using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.ConfigDefaultData
{
	public class HotelConfigrationData : IEntityTypeConfiguration<Hotel>
	{
		public void Configure(EntityTypeBuilder<Hotel> builder)
		{
			builder.HasData(new Hotel
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
