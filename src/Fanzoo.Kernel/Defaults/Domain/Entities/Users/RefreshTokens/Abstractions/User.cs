using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens
{
    public abstract class User<TRoleValue>(int maxFailedLogins, int numberOfInactiveTokensToStore) : User<UserIdentifierValue, Guid, EmailUsernameValue, TRoleValue, Guid, RefreshToken, RefreshTokenIdentifierValue, Guid>(maxFailedLogins, numberOfInactiveTokensToStore)
        where TRoleValue : IRoleValue<Guid>
    {
        protected override RefreshToken CreateToken(DateTime expirationDate, IPAddressValue ipAddress) => RefreshToken.Create(expirationDate, ipAddress).Value;

    }
}
