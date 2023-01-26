using AutoMapper;
using HotelListing.API.Core.Dtos.Country;
using HotelListing.API.Core.Dtos.Hotel;
using HotelListing.API.Core.Dtos.Users;
using HotelListing.API.Data.Models;

namespace HotelListing.API.Core.Configration
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
