namespace Fanzoo.Kernel.Services
{
    public class HttpContextAccessorService(IHttpContextAccessor httpContextAccessor) : IContextAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    }
}
