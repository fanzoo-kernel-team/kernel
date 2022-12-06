using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;

namespace Fanzoo.Kernel.Domain.Entities.RefreshTokens.Guid
{
    public class RefreshToken : RefreshToken<RefreshTokenIdentifierValue, System.Guid, UserIdentifierValue, System.Guid>
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
