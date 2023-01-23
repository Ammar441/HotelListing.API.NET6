using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Dtos.Users
{
    public class ApiUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters")]
        public string Password { get; set; }
    }
}
