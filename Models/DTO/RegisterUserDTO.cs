namespace BookApi_MySQL.Models.DTO
{
    public class RegisterUserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";

        public string FullName { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }
    }
}
