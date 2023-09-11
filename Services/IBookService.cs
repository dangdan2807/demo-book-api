using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book?> AddBook(AddBookViewModel book);
        Task<Book?> GetBookById(int id);
        Task<Book?> UpdateBook(int id, UpdateBookViewModel book);
        Task<Book?> DeleteBook(int id);
    }
}
