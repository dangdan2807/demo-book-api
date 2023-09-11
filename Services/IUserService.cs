using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IUserService
    {
        Task<User> GetById(int userId);
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task<User> Register(RegisterViewModel user);
        Task<string> Login(LoginViewModel loginViewModel);
    }
}
