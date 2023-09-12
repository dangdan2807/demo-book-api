using BookApi_MySQL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi_MySQL.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Book name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Book name must be between 3 and 50 characters")]
        [Column("book_name")]
        public string bookName { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Column("price")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "catgory must be between 3 and 50 characters")]
        [Column("category")]
        public string category { get; set; } = null!;

        [Required(ErrorMessage = "author is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "author must be between 3 and 50 characters")]
        [Column("author")]
        public string author { get; set; } = null!;

        [Column("is_deleted")]
        public bool isDeleted { get; set; } = false;

        [ForeignKey("user_id")]
        public User user;
    }
}