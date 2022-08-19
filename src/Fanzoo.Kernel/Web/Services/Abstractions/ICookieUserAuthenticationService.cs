using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.Web.Services
{
    public interface ICookieUserAuthenticationService
    {

        public Task ValidateLastAuthenticationChangeAsync(CookieValidatePrincipalContext context);
    }
}