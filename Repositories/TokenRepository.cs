using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;
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

        public async Task<TokenResponse> getTokenByAccessTokenAndRefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            var token = await _context.TokenResponses
                .Where(
                    t => t.accessToken == refreshTokenViewModel.accessToken
                    && t.refreshToken == refreshTokenViewModel.refreshToken
                )
                .FirstOrDefaultAsync();
            return token;
        }

        public async Task<TokenResponse?> GetTokenByUserIdAndDate(int userId, DateTime time)
        {
            var token = await _context.TokenResponses
                .Where(t => t.userId == userId && t.expRefreshToken >= time)
                .OrderBy(t => t.expRefreshToken)
                .FirstOrDefaultAsync();
            return token;
        }

        public async Task<TokenResponse> UpdateToken(int userId, TokenResponse token)
        {
            var existingToken = await _context.TokenResponses
                .Where(t => t.userId == userId)
                .FirstOrDefaultAsync();
            if (existingToken == null)
            {
                throw new ArgumentException("Token not found");
            }
            if (token.userId != userId)
            {
                throw new ArgumentException("User Id mismatch");
            }
            if (token.expRefreshToken < DateTime.Now)
            {
                throw new ArgumentException("token expired");
            }
            _context.Entry(token).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<TokenResponse?> getTokenByAccessToken(string accessToken)
        {
            var token = await _context.TokenResponses
                .Where(t => t.accessToken == accessToken)
                .FirstOrDefaultAsync();
            if (token == null)
            {
                return null;
            }
            return token;
        }
    }
}
