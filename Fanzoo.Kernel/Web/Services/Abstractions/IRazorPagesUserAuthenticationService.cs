#pragma warning disable S2326 // Unused type parameters should be removed

using Fanzoo.Kernel.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Fanzoo.Kernel.Web.Services
{
    public interface IRazorPagesUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword>
        where TUser : IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TPassword : IPasswordValue
    {
        ValueTask<UnitResult<Error>> SignInAsync(TUsername username, TPassword password);

        ValueTask SignOutAsync(TIdentifier identifier);

        public Task ValidateLastAuthenticationChangeAsync(CookieValidatePrincipalContext context);

    }
}