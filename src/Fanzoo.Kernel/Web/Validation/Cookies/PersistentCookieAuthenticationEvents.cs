using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.Web.Validation.Cookies
{
    public class PersistentCookieAuthenticationEvents(ICookieUserAuthenticationService userAuthenticationService) : CookieAuthenticationEvents
    {
        private readonly ICookieUserAuthenticationService _userAuthenticationService = userAuthenticationService;

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context) => _userAuthenticationService.ValidateLastAuthenticationChangeAsync(context);
    }
}
