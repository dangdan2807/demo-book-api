
using BookApi_MySQL.Contraint;
using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
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
        [Authorize(Roles = RoleContraint.Admin + ", " + RoleContraint.User)]
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
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) + "";
                    int userIdInt = Int32.Parse(userId + "");
                    existingBooks = await _bookService.GetBooksByUserId(userIdInt, pageNumber, pageSize, sort);
                }
                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Get all books successfully",
                    data = existingBooks
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> AddBook(AddBookViewModel addBookViewModel)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) + "";
                int userIdInt = Int32.Parse(userId);
                var book = await _bookService.AddBook(userIdInt, addBookViewModel);
                var apiResponse = new ApiResponse
                {
                    success = true,
                    message = "Add book successfully",
                    data = book
                };
                return CreatedAtAction(nameof(GetBookById), new { id = book.id }, apiResponse);
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    success = false,
                    message = ex.Message
                };
                return BadRequest(apiResponse);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                GetBookDTO? existingBook = null;
                existingBook = await _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "Book is not found"
                    });
                }

                if (role == "User")
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) + "";
                    int userIdInt = Int32.Parse(userId);
                    if (existingBook.createBy != userIdInt)
                    {
                        return Forbid();
                    }
                }

                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Get book successfully",
                    data = existingBook
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookViewModel updateBookViewModel)
        {
            try
            {
                var role = User.FindFirstValue(ClaimTypes.Role) + "";
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) + "";
                int userIdInt = Int32.Parse(userId);
                var userLogin = await _userRepository.GetUserById(userIdInt);
                if (userLogin == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "user is not found"
                    });
                }

                GetBookDTO? existingBook = await _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "book is not found"
                    });
                }
                if (role == "User")
                {
                    if (existingBook.userId != userIdInt)
                    {
                        return Forbid();
                    }
                }

                var updatedBook = await _bookService.UpdateBook(id, userIdInt, role, updateBookViewModel);
                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Update book successfully",
                    data = updatedBook
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                string role = User.FindFirstValue(ClaimTypes.Role) + "";
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) + "";
                int userIdInt = Int32.Parse(userId);
                GetBookDTO? existingBook = await _bookService.DeleteBook(id, userIdInt, role);
                if (existingBook == null)
                {
                    return NotFound(new ApiResponse
                    {
                        success = false,
                        message = "Book is not found"
                    });
                }
                if (role == "User")
                {
                    if (existingBook.createBy != userIdInt)
                    {
                        return Forbid();
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
