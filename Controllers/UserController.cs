using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                User? createdUser = await _userService.Register(registerViewModel);
                RegisterUserDTO registerUserDTO = new RegisterUserDTO
                {
                    UserId = createdUser.UserId,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    Phone = createdUser.Phone,
                    FullName = createdUser.FullName,
                    DateOfBirth = createdUser.DateOfBirth
                };
                return Ok(registerUserDTO);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var user = await _userService.GetById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var jwtToken = await _userService.Login(loginViewModel);
                return Ok(new
                {
                    jwtToken
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { e.Message });
            }
        }
    }
}
