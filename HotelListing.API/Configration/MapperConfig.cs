using AutoMapper;
using HotelListing.API.Dtos.Country;
using HotelListing.API.Dtos.Hotel;
using HotelListing.API.Dtos.Users;
using HotelListing.API.Models;

namespace HotelListing.API.Configration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

            CreateMap<ApiUser, ApiUserDto>().ReverseMap();
        }
    }
}
