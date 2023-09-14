using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserById(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);
            return user;
        }

        public Task<User> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
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
