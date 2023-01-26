using AutoMapper;
using HotelListing.API.Core.IRepository;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
	public class CountryRepository : GenericRepository<Country>, ICountryRepository
	{
		private readonly HotelListingDbContext _context;

		public CountryRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
			this._context = context;
		}

		public async Task<Country> GetDetailsAsync(int id)
		{
			return await _context.Countries.Include(m => m.Hotels).FirstOrDefaultAsync(m => m.Id == id);
		}
	}
}
