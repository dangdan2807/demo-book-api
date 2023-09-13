namespace BookApi_MySQL.Models.DTO
{
    public class AddBookDTO
    {
        public int Id { get; set; }
        public string bookName { get; set; } = null!;
        public decimal price { get; set; }
        public string category { get; set; } = null!;
        public string author { get; set; } = null!;
        public bool isDeleted { get; set; } = false;
        public int? UserId { get; set; }
    }
}
