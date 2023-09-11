namespace BookApi_MySQL.ViewModel
{
    public class UpdateBookViewModel
    {
        public string? bookName { get; set; }

        public decimal? price { get; set; } = null!;

        public string? category { get; set; }

        public string? author { get; set; }
    }
}
