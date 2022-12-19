namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextAccessorExtensions
    {
        private const string UNKNOWN_IP = "0.0.0.0";

        public static string GetIPv4Address(this IHttpContextAccessor httpContextAccessor)
        {
            var ip = UNKNOWN_IP;

            if (httpContextAccessor.HttpContext is not null)
            {
                if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    ip = ((string?)httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]) ?? UNKNOWN_IP;
                }
                else
                {
                    if (httpContextAccessor.HttpContext.Connection.RemoteIpAddress is not null)
                    {
                        ip = (httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()) ?? UNKNOWN_IP;
                    }
                }
            }

            return ip;
        }
    }
}
