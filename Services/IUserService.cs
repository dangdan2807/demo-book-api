using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> Register(RegisterViewModel user);
        Task<LoginDTO> Login(LoginViewModel loginViewModel);
        Task<LoginDTO> RefreshToken(RefreshTokenViewModel refreshTokenViewModel);
    }
}
