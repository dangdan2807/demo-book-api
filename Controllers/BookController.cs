
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace TodoApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly Serilog.ILogger _serilogLogger;
        private readonly IUserRepository _userRepository;

        public BookController(IBookService bookService, Serilog.ILogger serilogLogger, IUserRepository userRepository)
        {
            _bookService = bookService;
            _serilogLogger = serilogLogger;
            _userRepository = userRepository;
        }

        // policy: Admin, User
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetBooks(int? pageNumber = 1, int? pageSize = 10, string? sort = "ASC")
        {
            try
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                GetBooksDTO existingBooks = null;
                if (role == "Admin")
                {
                    existingBooks = await _bookService.GetBooks(pageNumber, pageSize, sort);
                }
                else
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    int userIdInt = Int32.Parse(userId);
                    existingBooks = await _bookService.GetBooksByUserId(userIdInt, pageNumber, pageSize, sort);
                }
                return Ok(existingBooks);
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
                return CreatedAtAction(nameof(GetBookById), new { id = book.id }, book);
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
                var role = User.FindFirstValue(ClaimTypes.Role);
                GetBookDTO existingBook = null;
                if (role == "Admin")
                {
                    existingBook = await _bookService.GetBookById(id);
                }
                else
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    int userIdInt = Int32.Parse(userId);
                    existingBook = await _bookService.GetBookByIdAndUserId(id, userIdInt);
                    if (existingBook == null)
                    {
                        return Forbid();
                    }
                }
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
                var role = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = Int32.Parse(userId);
                var userLogin = await _userRepository.GetUserById(userIdInt);
                if (userLogin == null)
                {
                    _serilogLogger.Error("user is not found");
                    return NotFound(JsonSerializer.Serialize(new
                    {
                        message = "user is not found"
                    }));
                }
                GetBookDTO existingBook = await _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    _serilogLogger.Error("book is not found");
                    return NotFound(JsonSerializer.Serialize(new
                    {
                        message = "book is not found"
                    }));
                }
                if (role == "User")
                {
                    if (existingBook.userId != userIdInt)
                    {
                        return Forbid(JsonSerializer.Serialize(new
                        {
                            message = "Bạn không có quyền để cập nhật cuốn sách này"
                        }));
                    }
                }
                var updatedBook = await _bookService.UpdateBook(id, userIdInt, role, updateBookViewModel);
                return Ok(existingBook);
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
                string role = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = Int32.Parse(userId);
                GetBookDTO existingBook = await _bookService.DeleteBook(id, userIdInt, role);
                if (existingBook == null)
                {
                    return NotFound();
                }
                if (role == "User")
                {
                    if (existingBook != null)
                    {
                        return Forbid();
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
