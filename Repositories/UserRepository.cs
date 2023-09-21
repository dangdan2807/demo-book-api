using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookApi_MySQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> Create(RegisterViewModel registerViewModel)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password, salt);
            var user = new User
            {
                username = registerViewModel.Username ?? "",
                email = registerViewModel.Email,
                phone = registerViewModel.Phone,
                fullName = registerViewModel.FullName ?? "",
                dateOfBirth = registerViewModel.DateOfBirth,
                HashedPassword = hashedPassword
            };

            var savedUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return savedUser.Entity;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.username == username);
        }
    }
}
