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

		public AuthManager(UserManager<ApiUser> userManager, IMapper mapper, IConfiguration configuration)
		{
			this._userManager = userManager;
			this._mapper = mapper;
			this._configuration = configuration;
		}

		public async Task<AuthResponseDto> Login(LoginDto loginDto)
		{
			bool isValidCredentials = false;

			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user is null)
			{
				return null;
			}
			isValidCredentials = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (!isValidCredentials)
			{
				return null;
			}
			var token = await GenerateToken(user);
			return new AuthResponseDto
			{
				Token = token,
				UserId = user.Id,
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

		private async Task<string> GenerateToken(ApiUser user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
			var userClaims = await _userManager.GetClaimsAsync(user);
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim("uid",user.Id)
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
