namespace HotelListing.API.Dtos.Hotel
{
    public class CreateHotelDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        public int CountryId { get; set; }
    }
    public class GetHotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        public int CountryId { get; set; }
        //public GetCountryDto Country { get; set; }
    }
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        public int CountryId { get; set; }
        //public GetCountryDto Country { get; set; }
    }
}
