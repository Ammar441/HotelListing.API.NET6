﻿using AutoMapper;
using HotelListing.API.Dtos.Users;
using HotelListing.API.IRepository;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Repository
{
	public class AuthManager : IAuthManager
	{
		private readonly UserManager<ApiUser> _userManager;
		private readonly IMapper _mapper;

		public AuthManager(UserManager<ApiUser> userManager, IMapper mapper)
		{
			this._userManager = userManager;
			this._mapper = mapper;
		}

		public async Task<bool> Login(LoginDto loginDto)
		{
			bool isValidCredentials = false;
			try
			{
				var user = await _userManager.FindByEmailAsync(loginDto.Email);
				if (user is null)
				{
					return default;
				}
				isValidCredentials = await _userManager.CheckPasswordAsync(user, loginDto.Password);
				if (!isValidCredentials)
				{
					return default;
				}

			}
			catch (Exception ex)
			{

			}
			return isValidCredentials;
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
	}
}
