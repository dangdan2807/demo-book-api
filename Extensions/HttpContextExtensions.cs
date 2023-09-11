namespace BookApi_MySQL.Extensions
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            return httpContext.Items["userId"] as int? ??
                throw new Exception("User ID not found in HttpContext.Items");
        }
    }
}
