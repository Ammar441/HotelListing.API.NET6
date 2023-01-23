using HotelListing.API.Dtos.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.IRepository
{
	public interface IAuthManager
	{
		Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto);
		Task<AuthResponseDto> Login(LoginDto loginDto);
		Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
		Task<string> CreateRefreshToken();
	}
}
