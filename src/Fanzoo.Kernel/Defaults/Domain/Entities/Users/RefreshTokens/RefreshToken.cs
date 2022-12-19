using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens
{
    public class RefreshToken : RefreshToken<RefreshTokenIdentifierValue, Guid>
    {
        public static ValueResult<RefreshToken, Error> Create(DateTime expirationDate, IPAddressValue ipAddress) =>
            expirationDate <= SystemDateTime.Now
                ? Errors.Entities.RefreshToken.ExpirationDateMustBeInTheFuture
                : new RefreshToken
                {
                    ExpirationDate = expirationDate,
                    IPAddress = ipAddress,
                    Issued = SystemDateTime.Now,
                    Token = RefreshTokenValue.Generate()
                };

    }
}
