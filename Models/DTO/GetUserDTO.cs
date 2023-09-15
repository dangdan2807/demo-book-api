namespace BookApi_MySQL.Models.DTO
{
    public class GetUserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string FullName { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
