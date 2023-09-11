using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetById(int userId);
        Task<User?> GetByUsername(string username);
        Task<User?> GetByEmail(string email);
        Task<User?> Create(RegisterViewModel registerViewModel);
        Task<string> Login(LoginViewModel loginViewModel);
    }
}
