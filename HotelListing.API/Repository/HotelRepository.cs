using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.IRepository;
using HotelListing.API.Models;

namespace HotelListing.API.Repository
{
	public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
	{

		public HotelRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
		}
	}
}
