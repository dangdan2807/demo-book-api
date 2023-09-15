using BookApi_MySQL.Models;
using BookApi_MySQL.Models.DTO;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface IUserService
    {
        Task<GetUserDTO> GetUserById(int userId);
        Task<GetUserDTO> GetUserByUsername(string username);
        Task<GetUserDTO> GetUserByEmail(string email);
        Task<User> Register(RegisterViewModel user);
        Task<LoginDTO> Login(LoginViewModel loginViewModel);
        Task<LoginDTO> RefreshToken(RefreshTokenViewModel refreshTokenViewModel);
    }
}
