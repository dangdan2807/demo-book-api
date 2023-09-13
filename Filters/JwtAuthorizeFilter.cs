//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Text;

//namespace BookApi_MySQL.Filters
//{
//    public class JwtAuthorizeFilter : IAuthorizationFilter
//    {
//        private readonly IConfiguration _config;

//        public JwtAuthorizeFilter(IConfiguration config)
//        {
//            _config = config;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var token = context.HttpContext.Request
//                .Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//            if (token == null)
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_config["Jwt:SercetKey"] ?? "");
//            try
//            {
//                tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    //Nếu token hết hạn,
//                    //thì khi gọi phương thức ValidateToken,
//                    //mã lỗi SecurityTokenExpiredException sẽ được throw ra
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);
//                var jwtToken = (JwtSecurityToken)validatedToken;
//                if (jwtToken.ValidTo < DateTime.UtcNow)
//                {
//                    // Token đã hết hạn
//                    // Xử lý lỗi hoặc đăng nhập lại để tạo mới token
//                    context.Result = new UnauthorizedResult();
//                    return;
//                }
//                var userId = int.Parse(jwtToken.Claims.First().Value);
//                var role = jwtToken.Claims.Select(x => x.Value).ToList()[1];
//                Console.WriteLine($"userId: {userId}, role: {role}");
//                context.HttpContext.Items["userId"] = userId;
//                //context.HttpContext.Items["role"] = role;
//            }
//            catch (Exception)
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }
//        }
//    }
//}