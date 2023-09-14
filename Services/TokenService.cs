using BookApi_MySQL.Models;
using BookApi_MySQL.Repositories;
using BookApi_MySQL.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookApi_MySQL.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public TokenService(ITokenRepository tokenRepository, IUserRepository userRepository, IConfiguration config)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
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

        public async Task<string> GenerateRefreshToken(int userId, string token)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                var now = DateTime.UtcNow;
                var tokenResponse = new TokenResponse
                {
                    userId = userId,
                    refreshToken = refreshToken,
                    accessToken = token,
                    expRefreshToken = DateTime.UtcNow.AddDays(365)
                };
                await _tokenRepository.AddToken(tokenResponse);
                return refreshToken;
            }
        }

        public async Task<TokenResponse> getTokenByAccessTokenAndRefreshToken(RefreshTokenViewModel refreshTokenViewModel)
        {
            var token = await _tokenRepository.getTokenByAccessTokenAndRefreshToken(refreshTokenViewModel);
            return token;
        }
    }
}
