using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IBookService
    {
        Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<GetBooksDTO> GetBooksByUserId(int userId, int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC");
        Task<GetBookDTO?> AddBook(int userId, AddBookViewModel book);
        Task<GetBookDTO?> GetBookById(int id);
        Task<GetBookDTO?> GetBookByIdAndUserId(int id, int userId);
        Task<GetBookDTO?> UpdateBook(int id, int userId, string role, UpdateBookViewModel book);
        Task<GetBookDTO?> DeleteBook(int id, int userId, string role);
    }
}
