using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookApi_MySQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<User?> Create(RegisterViewModel registerViewModel)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password, salt);
            var user = new User
            {
                Username = registerViewModel.Username ?? "",
                Email = registerViewModel.Email,
                Phone = registerViewModel.Phone,
                FullName = registerViewModel.FullName ?? "",
                DateOfBirth = registerViewModel.DateOfBirth,
                HashedPassword = hashedPassword
            };

            var savedUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return savedUser.Entity;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<string> Login(LoginViewModel loginViewModel)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.Email == loginViewModel.Email);
            if (user == null)
            {
                throw new ArgumentException("Wrong email or password");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.HashedPassword);
            if (!isPasswordValid)
            {
                throw new ArgumentException("Wrong email or password");
            }

            // Nếu xác thực thành công thì trả về chuỗi JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SercetKey"] ?? "");
            var role = user.IsAdmin ? "Admin" : "User";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                               SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
