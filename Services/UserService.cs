using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;
using Microsoft.IdentityModel.Tokens;

namespace BookApi_MySQL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _config = configuration;
        }

        public async Task<GetUserDTO> GetUserByEmail(string email)
        {
            User? user = await _userRepository.GetUserByEmail(email);
            return new GetUserDTO
            {
                UserId = user.userId,
                Username = user.username,
                Email = user.email,
                Phone = user.phone,
                FullName = user.fullName,
                DateOfBirth = user.dateOfBirth
            };
        }

        public async Task<GetUserDTO?> GetUserById(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return null;
            }

            return new GetUserDTO
            {
                UserId = userId,
                Username = user.username,
                Email = user.email,
                Phone = user.phone,
                FullName = user.fullName,
                DateOfBirth = user.dateOfBirth
            };
        }

        public async Task<GetUserDTO> GetUserByUsername(string username)
        {
            User? user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return null;
            }

            return new GetUserDTO
            {
                UserId = user.userId,
                Username = user.username,
                Email = user.email,
                Phone = user.phone,
                FullName = user.fullName,
                DateOfBirth = user.dateOfBirth
            };
        }

        public async Task<LoginDTO> Login(LoginViewModel loginViewModel)
        {
            User? user = await _userRepository.GetUserByEmail(loginViewModel.Email);
            if (user == null)
            {
                throw new ArgumentException("Wrong email or password");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.HashedPassword);
            if (!isPasswordValid)
            {
                throw new ArgumentException("Wrong email or password");
            }

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.userId, accessToken);

            return new LoginDTO
            {
                accessToken = accessToken,
                refreshToken = refreshToken
            };
        }

        public async Task<LoginDTO> RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            var token = await _tokenService.getTokenByAccessTokenAndRefreshToken(refreshTokenViewModel);
            if (token != null)
            {
                if (token.expRefreshToken < DateTime.UtcNow)
                {
                    throw new SecurityTokenException("token has expired");
                }

                var user = await _userRepository.GetUserById(token.userId);
                if (user == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var newToken = await _tokenService.RenewAccessToken(user, token);
                return new LoginDTO
                {
                    accessToken = newToken.accessToken,
                    refreshToken = newToken.refreshToken
                };
            }
            else
            {
                // Unauthorized
                throw new SecurityTokenException("Invalid token");
            }
        }

        public async Task<User?> Register(RegisterViewModel registerViewModel)
        {
            var existingUserByUsername = await _userRepository.GetUserByUsername(registerViewModel.Username);
            if (existingUserByUsername != null)
            {
                throw new ArgumentException("Username already exists");
            }
            var existingUserByEmail = await _userRepository.GetUserByEmail(registerViewModel.Email);
            if (existingUserByEmail != null)
            {
                throw new ArgumentException("Email already exists");
            }

            User? createdUser = await _userRepository.Create(registerViewModel);
            return createdUser;
        }

        public async Task Logout(string accessToken)
        {
            if (accessToken == null)
            {
                throw new ArgumentException("Access token is null");
            }
            await _tokenService.RevokeToken(accessToken);
        }
    }
}
