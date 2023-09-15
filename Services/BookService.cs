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

        public BookService(IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<AddBookDTO?> AddBook(int userId, AddBookViewModel addBookViewModel)
        {
            var existingBookById = await _bookRepository.GetBookById(addBookViewModel.Id ?? 0);
            if (existingBookById != null)
            {
                throw new ArgumentException("Book id already exists");
            }

            var existingBookByName = await _bookRepository.GetBookByName(addBookViewModel.bookName);
            if (existingBookByName != null)
            {
                throw new ArgumentException("Book name already exists");
            }

            var existingUser = await _userRepository.GetUserById(userId);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found");
            }

            var book = new Book
            {
                Id = addBookViewModel.Id ?? 0,
                bookName = addBookViewModel.bookName,
                price = addBookViewModel.price,
                category = addBookViewModel.category,
                author = addBookViewModel.author,
                user = existingUser
            };
            var newBook = await _bookRepository.AddBook(book);

            var addBookDTO = new AddBookDTO
            {
                Id = newBook.Id,
                bookName = newBook.bookName,
                price = newBook.price,
                category = newBook.category,
                author = newBook.author,
                UserId = newBook.user.UserId
            };
            return addBookDTO;
        }

        public Task<Book?> GetBookById(int id)
        {
            try
            {
                var book = _bookRepository.GetBookById(id);
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<Book?> GetBookByIdAndUserId(int id, int userId)
        {
            try
            {
                var book = _bookRepository.GetBookByIdAndUserId(id, userId);
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetBooksDTO> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            var books = await _bookRepository.GetBooks(pageNumber, pageSize, sort);
            return books;
        }

        public async Task<Book?> UpdateBook(int id, UpdateBookViewModel book)
        {
            var existingBook = await _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }
            existingBook.bookName = book.bookName;
            existingBook.price = (decimal)book.price;
            existingBook.category = book.category;
            existingBook.author = book.author;
            return await _bookRepository.UpdateBook(existingBook);
        }

        public async Task<Book?> UpdateBookByIdAndUserId(int id, int userId, UpdateBookViewModel book)
        {
            var existingBook = await _bookRepository.GetBookByIdAndUserId(id, userId);
            if (existingBook == null)
            {
                return null;
            }
            existingBook.bookName = book.bookName;
            existingBook.price = (decimal)book.price;
            existingBook.category = book.category;
            existingBook.author = book.author;
            return await _bookRepository.UpdateBook(existingBook);
        }

        public async Task<Book?> DeleteBook(int id)
        {
            try
            {
                var existingBook = await _bookRepository.GetBookById(id);
                if (existingBook == null)
                {
                    throw new Exception("Book not found");
                }
                var book = await _bookRepository.DeleteBook(id);
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Book?> DeleteBookByIdAndUserId(int id, int userId)
        {
            var existingBook = await _bookRepository.GetBookByIdAndUserId(id, userId);
            if (existingBook == null)
            {
                return null;
            }
            var book = await _bookRepository.DeleteBook(id);
            return book;
        }
    }
}
