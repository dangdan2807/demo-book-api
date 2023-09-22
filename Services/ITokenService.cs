using BookApi_MySQL.Models;
using BookApi_MySQL.ViewModel;

namespace BookApi_MySQL.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken(int userId, string token);
        Task<TokenResponse> getTokenByAccessTokenAndRefreshToken(RefreshTokenViewModel refreshTokenViewModel);
        Task<TokenResponse> getTokenByAccessToken(string accessToken);
        Task<TokenResponse> RenewAccessToken(User user, TokenResponse tokenResponse);
        Task RevokeToken(string accessToken);
    }
}
