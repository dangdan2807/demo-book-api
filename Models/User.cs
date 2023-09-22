using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int userId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters")]
        [Column("username")]
        public string username { get; set; } = "";

        [Required(ErrorMessage = "HashedPassword is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "HashedPassword must be between 6 and 200 characters")]
        [Column("hashed_password")]
        public string HashedPassword { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Column("email")]
        public string email { get; set; } = "";

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\+?(\d{10,15})$", ErrorMessage = "Invalid phone number")]
        [Column("phone")]
        public string phone { get; set; } = "";

        [StringLength(255, ErrorMessage = "FullName cannot exceed 255 characters")]
        [Column("full_name")]
        public string fullName { get; set; } = "";

        [Column("date_of_birth")]
        public DateTime? dateOfBirth { get; set; }

        [Column("is_admin")]
        public bool isAdmin { get; set; } = false;

        // navication
        public ICollection<Book>? books { get; set; }
    }
}
