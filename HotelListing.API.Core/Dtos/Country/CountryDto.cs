using HotelListing.API.Core.Dtos.Hotel;

namespace HotelListing.API.Core.Dtos.Country
{
	public class CountryDto : BaseCountryDto
	{
		public int Id { get; set; }
		public IList<HotelDto> Hotels { get; set; }
	}
}
