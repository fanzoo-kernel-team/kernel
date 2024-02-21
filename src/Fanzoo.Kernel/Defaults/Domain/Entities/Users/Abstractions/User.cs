using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users
{
    public abstract class User<TRoleValue>(int maxFailedLogins) : User<UserIdentifierValue, Guid, EmailUsernameValue, TRoleValue, Guid>(maxFailedLogins)
        where TRoleValue : IRoleValue<Guid>
    {
    }
}
