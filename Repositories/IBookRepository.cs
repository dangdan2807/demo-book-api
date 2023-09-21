using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;

namespace BookApi_MySQL.Repositories
{
    public interface IBookRepository
    {
        Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<GetBooksDTO> GetBooksByUserId(int userId, int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<Book?> GetBookById(int id);
        Task<Book?> GetBookByIdAndUserId(int id, int userId);
        Task<Book?> AddBook(int userId, Book book);
        Task<Book> UpdateBook(int id, int userId, Book book);
        Task<Book> DeleteBook(int id, int userId);
        Task<Book?> GetBookByName(string bookName);
    }
}
