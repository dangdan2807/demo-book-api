using BookApi_MySQL.Models;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetById(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);
            return user;
        }

        public Task<User> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            return await _userRepository.Login(loginViewModel);
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
