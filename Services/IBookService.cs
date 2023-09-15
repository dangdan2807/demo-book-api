using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IBookService
    {
        Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<AddBookDTO?> AddBook(int userId, AddBookViewModel book);
        Task<Book?> GetBookById(int id);
        Task<Book?> GetBookByIdAndUserId(int id, int userId);
        Task<Book?> UpdateBook(int id, UpdateBookViewModel book);
        Task<Book?> UpdateBookByIdAndUserId(int id, int userId, UpdateBookViewModel book);
        Task<Book?> DeleteBook(int id);
        Task<Book?> DeleteBookByIdAndUserId(int id, int userId);
    }
}
