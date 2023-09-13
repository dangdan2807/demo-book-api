using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;

namespace BookApi_MySQL.Repositories
{
    public interface IBookRepository
    {
        Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<IEnumerable<GetBooksDTO>> GetBooksByUserId(int userId);
        Task<Book?> GetBookById(int id);
        Task<Book?> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(int id);
        Task<Book?> GetBookByName(string bookName);
    }
}
