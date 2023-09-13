using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);
        Task<User?> Create(RegisterViewModel registerViewModel);
        Task<string> Login(LoginViewModel loginViewModel);
    }
}
