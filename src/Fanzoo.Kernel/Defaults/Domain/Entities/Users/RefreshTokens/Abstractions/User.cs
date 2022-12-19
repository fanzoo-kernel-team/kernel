
using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users.RefreshTokens
{
    public abstract class User<TRoleValue> : User<UserIdentifierValue, Guid, EmailUsernameValue, TRoleValue, Guid, RefreshToken, RefreshTokenIdentifierValue, Guid>
        where TRoleValue : IRoleValue<Guid>
    {
        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins, numberOfInactiveTokensToStore) { }

        protected override RefreshToken CreateToken(DateTime expirationDate, IPAddressValue ipAddress) => RefreshToken.Create(expirationDate, ipAddress).Value;

    }
}
