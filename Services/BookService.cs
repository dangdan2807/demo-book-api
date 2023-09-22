using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly Serilog.ILogger _serilogLogger;

        public BookService(IBookRepository bookRepository, IUserRepository userRepository, Serilog.ILogger serilogLogger)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _serilogLogger = serilogLogger;
        }

        public async Task<GetBookDTO?> AddBook(int userId, AddBookViewModel addBookViewModel)
        {
            var existingBookById = await _bookRepository.GetBookById(addBookViewModel.Id ?? 0);
            if (existingBookById != null)
            {
                throw new ArgumentException("Book id already exists");
            }

            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found");
            }

            var book = new Book
            {
                id = addBookViewModel.Id ?? 0,
                bookName = addBookViewModel.bookName,
                price = addBookViewModel.price,
                category = addBookViewModel.category,
                author = addBookViewModel.author,
                user = existingUser,
                createBy = userId,
                createAt = DateTime.Now,
            };
            var newBook = await _bookRepository.AddBook(userId, book);
            if (newBook == null)
            {
                return null;
            }

            var bookDTO = new GetBookDTO
            {
                id = newBook.id,
                bookName = newBook.bookName,
                price = newBook.price,
                category = newBook.category,
                author = newBook.author,
                userId = newBook.userId,
                createAt = newBook.createAt,
                updateBy = newBook.updateBy,
                createBy = newBook.createBy,
                updateAt = newBook.updateAt,
            };
            return bookDTO;
        }

        public async Task<GetBookDTO?> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return null;
            }
            GetBookDTO bookDTO = new GetBookDTO
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
            };
            return bookDTO;
        }

        public async Task<GetBookDTO?> GetBookByIdAndUserId(int id, int userId)
        {
            var book = await _bookRepository.GetBookByIdAndUserId(id, userId);
            if (book == null)
            {
                return null;
            }
            GetBookDTO bookDTO = new GetBookDTO
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
            };
            return bookDTO;
        }

        public async Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _bookRepository.GetBooks(pageNumber, pageSize, sort);
            return books;
        }

        public async Task<GetBooksDTO> GetBooksByUserId(int userId, int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _bookRepository.GetBooksByUserId(userId, pageNumber, pageSize, sort);
            return books;
        }

        public async Task<GetBookDTO?> UpdateBook(int id, int userId, string role, UpdateBookViewModel book)
        {
            Book? existingBook = null;
            if (role == "Admin")
            {
                existingBook = await _bookRepository.GetBookById(id);
            }
            else
            {
                existingBook = await _bookRepository.GetBookByIdAndUserId(id, userId);
            }
            if (existingBook == null)
            {
                return null;
            }
            existingBook.bookName = book.bookName;
            existingBook.price = (decimal)book.price;
            existingBook.category = book.category;
            existingBook.author = book.author;
            existingBook.updateAt = DateTime.Now;
            existingBook.updateBy = userId;

            var updateBook = await _bookRepository.UpdateBook(id, userId, existingBook);
            GetBookDTO bookDTO = new GetBookDTO
            {
                id = updateBook.id,
                bookName = updateBook.bookName,
                price = updateBook.price,
                category = updateBook.category,
                author = updateBook.author,
                userId = updateBook.userId,
                createAt = updateBook.createAt,
                updateBy = updateBook.updateBy,
                createBy = updateBook.createBy,
                updateAt = updateBook.updateAt,
            };
            return bookDTO;
        }

        public async Task<GetBookDTO?> DeleteBook(int id, int userId, string role)
        {
            Book existingBook = null;
            if (role == "Admin")
            {
                existingBook = await _bookRepository.GetBookById(id);
            }
            else
            {
                existingBook = await _bookRepository.GetBookByIdAndUserId(id, userId);
            }
            if (existingBook == null)
            {
                return null;
            }
            var book = await _bookRepository.DeleteBook(id, userId);

            GetBookDTO bookDTO = new GetBookDTO
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
            };
            return bookDTO;

        }
    }
}
