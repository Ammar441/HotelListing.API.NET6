using HotelListing.API.Data.Models;

namespace HotelListing.API.Core.IRepository
{
	public interface ICountryRepository : IGenericRepository<Country>
	{
		Task<Country> GetDetailsAsync(int id);
	}
}
