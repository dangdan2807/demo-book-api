using BookApi_MySQL.Models;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) => _bookRepository = bookRepository;

        public async Task<Book?> AddBook(AddBookViewModel addBookViewModel)
        {
            var existingBookById = await _bookRepository.GetBookById(addBookViewModel.Id ?? 0);
            if (existingBookById != null)
            {
                throw new Exception("Book id already exists");
            }

            var existingBookByName = await _bookRepository.GetBookByName(addBookViewModel.bookName);
            if (existingBookByName != null)
            {
                throw new Exception("Book name already exists");
            }
            var book = new Book
            {
                Id = addBookViewModel.Id ?? 0,
                bookName = addBookViewModel.bookName,
                price = addBookViewModel.price,
                category = addBookViewModel.category,
                author = addBookViewModel.author,
            };
            return await _bookRepository.AddBook(book);
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

        public Task<IEnumerable<Book>> GetBooks()
        {
            var books = _bookRepository.GetBooks();
            return books;
        }

        public async Task<Book?> UpdateBook(int id, UpdateBookViewModel book)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
