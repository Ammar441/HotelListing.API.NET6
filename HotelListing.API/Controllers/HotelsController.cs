using AutoMapper;
using HotelListing.API.Dtos.Hotel;
using HotelListing.API.IRepository;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class HotelsController : ControllerBase
	{
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;

		public HotelsController(IHotelRepository hotelRepository, IMapper mapper)
		{
			this._hotelRepository = hotelRepository;
			this._mapper = mapper;
		}

		// GET: api/Hotels
		[HttpGet]
		public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
		{
			var hotels = await _hotelRepository.GetAllAsync();
			return _mapper.Map<List<HotelDto>>(hotels);
		}

		// GET: api/Hotels/5
		[HttpGet("{id}")]
		public async Task<ActionResult<HotelDto>> GetHotel(int id)
		{
			var hotel = await _hotelRepository.GetAsync(id);

			if (hotel == null)
			{
				return NotFound();
			}

			return _mapper.Map<HotelDto>(hotel);
		}

		// PUT: api/Hotels/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutHotel(int id, HotelDto updateHotel)
		{
			if (id != updateHotel.Id)
			{
				return BadRequest();
			}
			var hotel = await _hotelRepository.GetAsync(id);
			if (hotel == null)
			{
				return NotFound();
			}
			_mapper.Map(updateHotel, hotel);
			try
			{
				await _hotelRepository.UpdateAsync(hotel);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await HotelExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Hotels
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotel)
		{
			var hotel = _mapper.Map<Hotel>(createHotel);
			await _hotelRepository.AddAsync(hotel);

			return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
		}

		// DELETE: api/Hotels/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteHotel(int id)
		{
			var hotel = await _hotelRepository.GetAsync(id);
			if (hotel == null)
			{
				return NotFound();
			}

			await _hotelRepository.DeleteAsync(id);

			return NoContent();
		}

		private async Task<bool> HotelExists(int id)
		{
			return await _hotelRepository.Exists(id);
		}
	}
}
