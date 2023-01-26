using HotelListing.API.Core.Dtos.Country;
using HotelListing.API.Data.Models;

namespace HotelListing.API.Core.IRepository
{
	public interface ICountryRepository : IGenericRepository<Country>
	{
		Task<CountryDto> GetDetailsAsync(int id);
	}
}
