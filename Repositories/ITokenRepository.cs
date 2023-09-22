using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;
using System;

namespace BookApi_MySQL.Repositories
{
    public interface ITokenRepository
    {
        Task<TokenResponse> AddToken(TokenResponse token);
        Task<TokenResponse> getTokenByAccessTokenAndRefreshToken(RefreshTokenViewModel refreshTokenViewModel);
        Task<TokenResponse> GetTokenByUserIdAndDate(int userId, DateTime time);
        Task<TokenResponse> UpdateToken(int userId, TokenResponse token);
        Task<TokenResponse?> getTokenByAccessToken(string accessToken);
    }
}
