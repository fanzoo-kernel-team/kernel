namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetIPv4Address(this IHttpContextAccessor httpContextAccessor) =>
            httpContextAccessor.HttpContext!.Request.Headers.ContainsKey("X-Forwarded-For")
                   ? httpContextAccessor.HttpContext!.Request.Headers["X-Forwarded-For"]!
                   : httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }
}
