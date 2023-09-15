using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
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

        public async Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _context.Books
                .Where(b => b.isDeleted == false)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
            // query count books
            var count = await _context.Books.CountAsync();

            var pagination = new PaginationDTO
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalCount = count
            };

            var getBooksDTOs = new GetBooksDTO { Books = books, Pagination = pagination };
            if (books == null || books.Count == 0)
            {
                getBooksDTOs = new GetBooksDTO { Books = new List<Book>(), Pagination = pagination };
            }

            return getBooksDTOs;
        }

        public async Task<GetBooksDTO> GetBooksByUserId(int userid, int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _context.Books
                .Where(b => b.isDeleted == false && b.UserId == userid)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
            // query count books
            var count = await _context.Books.CountAsync();

            var pagination = new PaginationDTO
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalCount = count
            };

            var getBooksDTOs = new GetBooksDTO { Books = books, Pagination = pagination };
            if (books == null || books.Count == 0)
            {
                getBooksDTOs = new GetBooksDTO { Books = new List<Book>(), Pagination = pagination };
            }

            return getBooksDTOs;
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<Book?> GetBookByIdAndUserId(int id, int userId)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false && b.UserId == userId)
                .FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task<Book?> AddBook(Book book)
        {
            var saveBook = _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return saveBook.Entity;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> DeleteBook(int id)
        {
            var book = await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.Id == id);
            book.isDeleted = true;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> GetBookByName(string bookName)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.bookName == bookName);
        }
    }
}
