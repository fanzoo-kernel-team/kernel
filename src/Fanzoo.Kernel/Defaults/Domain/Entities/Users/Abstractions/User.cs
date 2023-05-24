using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Defaults.Domain.Entities.Users
{
    public abstract class User<TRoleValue> : User<UserIdentifierValue, Guid, EmailUsernameValue, TRoleValue, Guid>
        where TRoleValue : IRoleValue<Guid>
    {
        protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    }
}
