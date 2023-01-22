using HotelListing.API.Models;

namespace HotelListing.API.IRepository
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetailsAsync(int id);
    }
}
