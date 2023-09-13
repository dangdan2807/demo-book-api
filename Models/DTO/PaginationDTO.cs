namespace BookApi_MySQL.Models.DTO
{
    public class PaginationDTO
    {
        public int? currentPage { get; set; }
        public int? pageSize { get; set; }
        public int? totalCount { get; set; }
    }
}
