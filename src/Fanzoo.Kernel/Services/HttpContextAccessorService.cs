namespace Fanzoo.Kernel.Services
{
    public class HttpContextAccessorService : IContextAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? GetUser() => _httpContextAccessor.HttpContext?.User;

        ValueTask<ClaimsPrincipal?> IContextAccessorService.GetUserAsync() => ValueTask.FromResult(_httpContextAccessor.HttpContext?.User);
    }
}
