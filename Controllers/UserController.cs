using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var getUserDTO = await _userService.GetUserById(userId);
                if (getUserDTO == null)
                {
                    return NotFound();
                }
                return Ok(getUserDTO);
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
                var loginDTO = await _userService.Login(loginViewModel);
                return Ok(loginDTO);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            try
            {
                var loginDTO = await _userService.RefreshToken(refreshTokenViewModel);
                return Ok(loginDTO);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { e.Message });
            } 
            catch (SecurityTokenException e)
            {
                return Unauthorized(new { e.Message });
            }
        }
    }
}
