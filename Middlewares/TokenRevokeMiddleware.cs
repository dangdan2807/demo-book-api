using BookApi_MySQL.Models;
using BookApi_MySQL.Repositories;
using System.Text.Json;

namespace BookApi_MySQL.Middlewares
{
    public class TokenRevokeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Serilog.ILogger _logger;

        public TokenRevokeMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, Serilog.ILogger logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Kiểm tra xem token có trong danh sách "blacklist" (revoke) không
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (await IsTokenRevokedAsync(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse
                {
                    success = false,
                    message = "Token has been revoked."
                }));
            }
            else
            {
                await _next(context);
            }
        }

        // Thực hiện truy vấn cơ sở dữ liệu để kiểm tra trường is_revoke
        // Trả về true nếu token đã bị revoke, ngược lại trả về false
        private async Task<bool> IsTokenRevokedAsync(string token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
                var existingToken = await _tokenRepository.getTokenByAccessToken(token);
                if (existingToken == null)
                {
                    return true;
                }
                if (existingToken.isRevoke)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
