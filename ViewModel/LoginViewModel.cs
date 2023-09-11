using System.ComponentModel.DataAnnotations;

namespace BookApi_MySQL.ViewModel
{
    public class LoginViewModel
    {
        //public string? Username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "HashedPassword is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "HashedPassword must be between 6 and 100 characters")]
        public string Password { get; set; } = "";
    }
}
