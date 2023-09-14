using BookApi_MySQL.Models;
using System;

namespace BookApi_MySQL.Repositories
{
    public interface ITokenRepository
    {
        Task<TokenResponse> AddToken(TokenResponse token);
        Task<TokenResponse> GetTokenByUserIdAndDate(int userId, DateTime time);
        Task<TokenResponse> UpdateToken(int userId, TokenResponse token);
    }
}
