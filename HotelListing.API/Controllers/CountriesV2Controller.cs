using AutoMapper;
using HotelListing.API.Core.Dtos.Country;
using HotelListing.API.Core.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers
{
	[Route("api/v{version:apiVersion}/countries")]
	[ApiController]
	[ApiVersion("2.0")]
	public class CountriesV2Controller : ControllerBase
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;

		public CountriesV2Controller(ICountryRepository countryRepository, IMapper mapper)
		{
			this._countryRepository = countryRepository;
			this._mapper = mapper;
		}

		// GET: api/Countries
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
		{
			var countries = await _countryRepository.GetAllAsync<CountryDto>();
			return Ok(countries);
		}

		// GET: api/Countries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<CountryDto>> GetCountry(int id)
		{
			var country = await _countryRepository.GetDetailsAsync(id);
			return Ok(country);
		}

		// PUT: api/Countries/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
		{
			if (id != updateCountry.Id)
			{
				return BadRequest();
			}
			try
			{
				await _countryRepository.UpdateAsync(id, updateCountry);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await CountryExists(id))
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

		// POST: api/Countries
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize(Roles = "Administrator")]
		public async Task<ActionResult<CreateCountryDto>> PostCountry(CreateCountryDto createCountry)
		{
			var country = await _countryRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountry);

			return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
		}

		// DELETE: api/Countries/5
		[HttpDelete("{id}")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> DeleteCountry(int id)
		{
			await _countryRepository.DeleteAsync(id);
			return NoContent();
		}

		private async Task<bool> CountryExists(int id)
		{
			return await _countryRepository.Exists(id);
		}
	}
}
