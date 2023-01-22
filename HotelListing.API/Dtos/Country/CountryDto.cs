using HotelListing.API.Dtos.Hotel;

namespace HotelListing.API.Dtos.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }
        public IList<HotelDto> Hotels { get; set; }
    }
}
