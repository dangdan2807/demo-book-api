using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth
            };
        }

        public async Task<GetUserDTO?> GetUserById(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);
            return new GetUserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth
            };
        }

        public async Task<GetUserDTO> GetUserByUsername(string username)
        {
            User? user = await _userRepository.GetUserByUsername(username);
            return new GetUserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth
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
            var refreshToken = await _tokenService.GenerateRefreshToken(user.UserId, accessToken);

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
                var user = await _userRepository.GetUserById(token.userId);
                if (user == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }
                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = await _tokenService.GenerateRefreshToken(user.UserId, accessToken);
                return new LoginDTO
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken
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
    }
}
