using BookApi_MySQL.Models;

namespace BookApi_MySQL.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book?> GetBookById(int id);
        Task<Book?> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(int id);
        Task<Book?> GetBookByName(string bookName);
    }
}
