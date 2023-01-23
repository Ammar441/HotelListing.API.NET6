using AutoMapper;
using HotelListing.API.Dtos.Users;
using HotelListing.API.IRepository;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
	public class AuthManager : IAuthManager
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private ApiUser _user;
		private const string _loginProvider = "HotelApiProviding";
		private const string _refreshToken = "RefreshToken";
		public AuthManager(UserManager<ApiUser> userManager, IMapper mapper, IConfiguration configuration)
		{
			this._userManager = userManager;
			this._mapper = mapper;
			this._configuration = configuration;
		}



		public async Task<AuthResponseDto> Login(LoginDto loginDto)
		{
			bool isValidCredentials = false;

			_user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (_user is null)
			{
				return null;
			}
			isValidCredentials = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
			if (!isValidCredentials)
			{
				return null;
			}
			var token = await GenerateToken();
			return new AuthResponseDto
			{
				Token = token,
				RefreshToken = await CreateRefreshToken(),
				UserId = _user.Id,
			};

		}

		public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
		{
			var user = _mapper.Map<ApiUser>(userDto);
			user.UserName = user.Email;
			var result = await _userManager.CreateAsync(user, userDto.Password);
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "User");
			}
			return result.Errors;
		}

		public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
			var userName = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type ==
			JwtRegisteredClaimNames.Email)?.Value;
			_user = await _userManager.FindByNameAsync(userName);
			if (_user == null || _user.Id != request.UserId)
			{
				return null;
			}
			var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
			if (isValidRefreshToken)
			{
				var token = await GenerateToken();
				return new AuthResponseDto
				{
					Token = token,
					RefreshToken = await CreateRefreshToken(),
					UserId = _user.Id,
				};
			}
			await _userManager.UpdateSecurityStampAsync(_user);
			return null;
		}
		public async Task<string> CreateRefreshToken()
		{
			await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
			var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
			var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
			return newRefreshToken;
		}
		private async Task<string> GenerateToken()
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var roles = await _userManager.GetRolesAsync(_user);
			var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
			var userClaims = await _userManager.GetClaimsAsync(_user);
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub,_user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email,_user.Email),
				new Claim("uid",_user.Id)
			}.Union(userClaims).Union(roleClaims);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationExpire"])),
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
