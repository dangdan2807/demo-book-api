using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Services;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Authentication;
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
        private readonly Serilog.ILogger _logger;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        public UserController(IUserService userService, Serilog.ILogger logger, IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _userService = userService;
            _logger = logger;
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                User? createdUser = await _userService.Register(registerViewModel);
                RegisterUserDTO registerUserDTO = new RegisterUserDTO
                {
                    UserId = createdUser.userId,
                    Username = createdUser.username,
                    Email = createdUser.email,
                    Phone = createdUser.phone,
                    FullName = createdUser.fullName,
                    DateOfBirth = createdUser.dateOfBirth
                };
                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Register successfully",
                    data = registerUserDTO
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = e.Message,
                });
            }
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int userId)
        {
            var apiResponse = new ApiResponse
            {
                success = true,
                message = "Get user successfully",
            };
            try
            {
                var getUserDTO = await _userService.GetUserById(userId);
                if (getUserDTO == null)
                {
                    apiResponse.success = false;
                    apiResponse.message = "User not found";
                    return NotFound(apiResponse);
                }
                apiResponse.data = getUserDTO;
                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                apiResponse.success = false;
                apiResponse.message = e.Message;
                return BadRequest(apiResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var loginDTO = await _userService.Login(loginViewModel);
                return Ok(new ApiResponse
                {
                    success = true,
                    message = "Login successfully",
                    data = loginDTO
                });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = e.Message,
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            var apiResponse = new ApiResponse
            {
                success = true,
                message = "Refresh token successfully",
            };
            try
            {
                var loginDTO = await _userService.RefreshToken(refreshTokenViewModel);
                apiResponse.data = loginDTO;
                return Ok(apiResponse);
            }
            catch (SecurityTokenException e)
            {
                apiResponse.success = false;
                apiResponse.message = e.Message;
                return Unauthorized(apiResponse);
            }
            catch (Exception e)
            {
                apiResponse.success = false;
                apiResponse.message = e.Message;
                return BadRequest(apiResponse);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // get access token from header
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new ApiResponse
                {
                    success = false,
                    message = "Access token is required"
                });
            }
            await _userService.Logout(accessToken);
            return NoContent();
        }
    }
}
