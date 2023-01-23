using HotelListing.API.Dtos.Users;
using HotelListing.API.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAuthManager _authManager;

		public AccountController(IAuthManager authManager)
		{
			this._authManager = authManager;
		}
		[HttpPost]
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Register([FromBody] ApiUserDto userDto)
		{
			var errors = await _authManager.Register(userDto);
			if (errors.Any())
			{
				foreach (var error in errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}
			return Ok(userDto);
		}
		[HttpPost]
		[Route("login")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var authResponse = await _authManager.Login(loginDto);
			if (authResponse == null)
			{
				return Unauthorized();
			}
			return Ok(authResponse);
		}
	}
}
