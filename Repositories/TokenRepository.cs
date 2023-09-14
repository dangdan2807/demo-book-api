using BookApi_MySQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi_MySQL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<TokenResponse> AddToken(TokenResponse token)
        {
            var saveToken = await _context.TokenResponses.AddAsync(token);
            await _context.SaveChangesAsync();
            return saveToken.Entity;
        }

        public Task<TokenResponse> GetTokenByUserIdAndDate(int userId, DateTime time)
        {
            var token = _context.TokenResponses
                .Where(t => t.userId == userId && t.expRefreshToken >= time)
                .OrderBy(t => t.expRefreshToken)
                .FirstOrDefaultAsync();
            return token;
        }

        public async Task<TokenResponse> UpdateToken(int userId, TokenResponse token)
        {
            _context.Entry(token).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return token;
        }
    }
}
