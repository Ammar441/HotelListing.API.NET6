namespace HotelListing.API.Core.Dtos.Users
{
	public class AuthResponseDto
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public string UserId { get; set; }
	}
}
