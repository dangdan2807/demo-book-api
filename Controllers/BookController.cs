
using BookApi_MySQL.Attribute;
using BookApi_MySQL.Extensions;
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TodoApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [JwtAuthorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService) => _bookService = bookService;

        [HttpGet]
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
        public async Task<IActionResult> AddBook(AddBookViewModel addBookViewModel)
        {
            try
            {
                int userId = HttpContext.GetUserId();
                var book = await _bookService.AddBook(userId, addBookViewModel);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
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
