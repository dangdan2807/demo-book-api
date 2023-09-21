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
            var sercetKey = Encoding.ASCII.GetBytes(_config["Jwt:SercetKey"] ?? "");
            var role = user.isAdmin ? "Admin" : "User";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(sercetKey),
                               SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        public async Task<TokenResponse> RenewAccessToken(User user, TokenResponse tokenResponse)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var sercetKey = Encoding.ASCII.GetBytes(_config["Jwt:SercetKey"] ?? "");
            var role = user.isAdmin ? "Admin" : "User";

            var tokenValidateParams = new TokenValidationParameters
            {
                // tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(sercetKey),
                ClockSkew = TimeSpan.Zero,

                // không check hết hạn token
                ValidateLifetime = false,
            };
            try
            {
                // access token valid format
                var tokenInVerification = tokenHandler.ValidateToken(tokenResponse.accessToken, tokenValidateParams, out var validatedToken);

                // check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, System.StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        throw new ArgumentException("Token algorithm is not match");
                    }
                }

                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    throw new ArgumentException("Access token hasn't expired yet");
                }

                // check refresh token
                var storedToken = await _tokenRepository.getTokenByAccessTokenAndRefreshToken(new RefreshTokenViewModel
                {
                    accessToken = tokenResponse.accessToken,
                    refreshToken = tokenResponse.refreshToken
                });
                if (storedToken == null)
                {
                    throw new ArgumentException("Refresh token not found");
                }

                // check refresh token is used/revoke
                if (storedToken.isUsed)
                {
                    throw new ArgumentException("Refresh token has been used");
                }
                if (storedToken.isRevoke)
                {
                    throw new ArgumentException("Refresh token has been revoked");
                }

                // update token is used
                string accessToken = await GenerateAccessToken(user);
                storedToken.accessToken = accessToken;
                storedToken.isUsed = true;
                storedToken.isRevoke = true;
                await _tokenRepository.UpdateToken(user.userId, storedToken);

                return new TokenResponse
                {
                    accessToken = accessToken,
                    refreshToken = tokenResponse.refreshToken
                };
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
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
                    expRefreshToken = DateTime.UtcNow.AddDays(7),
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

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
