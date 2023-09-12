using BookApi_MySQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi_MySQL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.Where(b => b.isDeleted == false).ToListAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books.Where(b => b.isDeleted == false).FirstAsync(book => book.Id == id);
        }

        public async Task<Book?> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> DeleteBook(int id)
        {
            var book = await _context.Books.Where(b => b.isDeleted == false).FirstAsync(book => book.Id == id);
            book.isDeleted = true;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> GetBookByName(string bookName)
        {
            return await _context.Books.Where(b => b.isDeleted == false).FirstOrDefaultAsync(book => book.bookName == bookName);
        }
    }
}
