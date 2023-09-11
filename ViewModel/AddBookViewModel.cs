using System.ComponentModel.DataAnnotations;

namespace BookApi_MySQL.ViewModel
{
    public class AddBookViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Book name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Book name must be between 3 and 50 characters")]
        public string bookName { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "catgory must be between 3 and 50 characters")]
        public string category { get; set; } = null!;

        [Required(ErrorMessage = "author is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "author must be between 3 and 50 characters")]
        public string author { get; set; } = null!;
    }
}
