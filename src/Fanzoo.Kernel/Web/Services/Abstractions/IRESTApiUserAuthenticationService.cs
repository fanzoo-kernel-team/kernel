#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable S2326 // Unused type parameters should be removed

using Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users;

namespace Fanzoo.Kernel.Web.Services
{
    public interface IRESTApiUserAuthenticationService<TUser, TIdentifier, TPrimitive, TUsername, TPassword, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
        where TUser : IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TPassword : IPasswordValue
        where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive, TIdentifier, TPrimitive>
        where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
        where TTokenPrimitive : notnull, new()
    {
        ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> AuthenticateAsync(TUsername username, TPassword password);

        ValueTask<ValueResult<(string AccessToken, string RefreshToken), Error>> RefreshTokenAsync(string refreshToken);

        ValueTask<UnitResult<Error>> RevokeAsync(string refreshToken);

        ValueTask<UnitResult<Error>> RevokeAllAsync(TIdentifier identifier);

    }
}