
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TodoApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger _logger;
        private readonly Serilog.ILogger _serilogLogger;

        public BookController(IBookService bookService, ILogger<BookController> logger
            , Serilog.ILogger serilogLogger
            )
        {
            _bookService = bookService;
            _logger = logger;
            _serilogLogger = serilogLogger;
        }

        // policy: Admin, User
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            try
            {
                var books = await _bookService.GetBooks(pageNumber, pageSize, sort);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> AddBook(AddBookViewModel addBookViewModel)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = Int32.Parse(userId);
                var book = await _bookService.AddBook(userIdInt, addBookViewModel);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var existingBook = await _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound();
                }
                return Ok(existingBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookViewModel updateBookViewModel)
        {
            try
            {
                var updatedBook = await _bookService.UpdateBook(id, updateBookViewModel);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBook(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
