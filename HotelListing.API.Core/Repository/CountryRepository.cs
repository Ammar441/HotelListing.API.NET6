using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Dtos.Country;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.IRepository;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
	public class CountryRepository : GenericRepository<Country>, ICountryRepository
	{
		private readonly HotelListingDbContext _context;
		private readonly IMapper _mapper;

		public CountryRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		public async Task<CountryDto> GetDetailsAsync(int id)
		{
			var country = await _context.Countries.Include(m => m.Hotels)
				.ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (country is null)
			{
				throw new NotFoundException(nameof(GetDetailsAsync), id);
			}
			return country;
		}
	}
}
