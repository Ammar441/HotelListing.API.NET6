using HotelListing.API.Data;
using HotelListing.API.IRepository;
using HotelListing.API.Models;

namespace HotelListing.API.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HotelListingDbContext _context;

        public HotelRepository(HotelListingDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
