using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BookApi_MySQL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly Serilog.ILogger _serilogLogger;

        public BookRepository(AppDbContext context, Serilog.ILogger serilogLogger)
        {
            _context = context;
            _serilogLogger = serilogLogger;
        }

        public async Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _context.Books
                .Where(b => b.isDeleted == false)
                .OrderBy(b => b.id)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
            // query count books
            var count = await _context.Books
                .Where(b => b.isDeleted == false)
                .OrderBy(b => b.id)
                .CountAsync();

            var pagination = new PaginationDTO
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalCount = count
            };

            var bookDTOs = books.Select(book => new GetBookDTO
            {
                id = book.id,
                bookName = book.bookName,
                price = book.price,
                category = book.category,
                author = book.author,
                userId = book.userId,
                createAt = book.createAt,
                updateBy = book.updateBy,
                createBy = book.createBy,
                updateAt = book.updateAt,
            }).ToList();

            var getBooksDTOs = new GetBooksDTO { Books = bookDTOs, Pagination = pagination };
            if (books == null || books.Count == 0)
            {
                getBooksDTOs = new GetBooksDTO { Books = new List<GetBookDTO>(), Pagination = pagination };
            }

            return getBooksDTOs;
        }

        public async Task<GetBooksDTO> GetBooksByUserId(int userid, int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _context.Books
                .Where(b => b.isDeleted == false && b.userId == userid)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
            // query count books
            var count = await _context.Books
                .Where(b => b.isDeleted == false && b.userId == userid)
                .CountAsync();

            var pagination = new PaginationDTO
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalCount = count
            };

            var bookDTOs = books.Select(book => new GetBookDTO
            {
                id = book.id,
                bookName = book.bookName,
                price = book.price,
                category = book.category,
                author = book.author,
                userId = book.userId,
                createAt = book.createAt,
                updateBy = book.updateBy,
                createBy = book.createBy,
                updateAt = book.updateAt,
            }).ToList();

            var getBooksDTOs = new GetBooksDTO { Books = bookDTOs, Pagination = pagination };
            if (books == null || books.Count == 0)
            {
                getBooksDTOs = new GetBooksDTO { Books = new List<GetBookDTO>(), Pagination = pagination };
            }

            return getBooksDTOs;
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.id == id);
        }

        public async Task<Book?> GetBookByIdAndUserId(int id, int userId)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false && b.userId == userId)
                .FirstOrDefaultAsync(book => book.id == id);
        }

        public async Task<Book?> AddBook(int userId, Book book)
        {
            book.userId = userId;
            book.createAt = DateTime.Now;
            var saveBook = _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return saveBook.Entity;
        }

        public async Task<Book> UpdateBook(int id, int userId, Book book)
        {
            var existingBook = await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.id == id);
            if (existingBook == null)
            {
                return null;
            }
            existingBook.deleteAt = DateTime.Now;
            existingBook.updateBy = userId;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> DeleteBook(int id, int userId)
        {
            var existingBook = await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.id == id);
            if (existingBook == null)
            {
                return null;
            }
            existingBook.isDeleted = true;
            existingBook.deleteAt = DateTime.Now;
            existingBook.updateBy = userId;
            _context.Entry(existingBook).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<Book?> GetBookByName(string bookName)
        {
            return await _context.Books
                .Where(b => b.isDeleted == false)
                .FirstOrDefaultAsync(book => book.bookName == bookName);
        }
    }
}
