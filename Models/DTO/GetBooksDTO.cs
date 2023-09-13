namespace BookApi_MySQL.Models.DTO
{
    public class GetBooksDTO
    {
        public ICollection<Book>? Books { get; set; }
        public PaginationDTO? Pagination { get; set; }
    }
}
