namespace BookApi_MySQL.Models.DTO
{
    public class GetBookDTO
    {
        public int id { get; set; }
        public string bookName { get; set; } = null!;
        public decimal price { get; set; }
        public string category { get; set; } = null!;
        public string author { get; set; } = null!;
        public int? userId { get; set; }
        public int createBy { get; set; }
        public int? updateBy { get; set; } = null;
        public DateTime createAt { get; set; }
        public DateTime? updateAt { get; set; } = null;
    }
}
