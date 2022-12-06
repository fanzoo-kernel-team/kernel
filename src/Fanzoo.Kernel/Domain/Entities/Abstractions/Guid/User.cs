using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;

namespace Fanzoo.Kernel.Domain.Entities.Users.Guid
{
    public abstract class User<TUsername> : User<UserIdentifierValue, System.Guid, TUsername>
        where TUsername : IUsernameValue

    {
        protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    }

    public abstract class User : User<UserIdentifierValue, System.Guid, EmailUsernameValue>
    {
        protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    }
}

namespace Fanzoo.Kernel.Domain.Entities.Users.Roles.Guid
{
    public abstract class User<TUsername, TRoleValue, TRolePrimitive> : User<UserIdentifierValue, System.Guid, TUsername, TRoleValue, TRolePrimitive>
        where TUsername : IUsernameValue
        where TRoleValue : IRoleValue<TRolePrimitive>
        where TRolePrimitive : notnull, new()
    {
        protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    }

    public abstract class User<TRoleValue, TRolePrimitive> : User<UserIdentifierValue, System.Guid, EmailUsernameValue, TRoleValue, TRolePrimitive>
        where TRoleValue : IRoleValue<TRolePrimitive>
        where TRolePrimitive : notnull, new()
    {
        protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    }

    //public abstract class User<TUsername, TRoleValue, TRolePrimitive> : User<UserIdentifierValue, System.Guid, TUsername, TRoleValue, TRolePrimitive>
    //    where TUsername : IUsernameValue
    //    where TRoleValue : IRoleValue<TRolePrimitive>
    //    where TRolePrimitive : notnull, new()
    //{
    //    protected User(int maxFailedLogins) : base(maxFailedLogins) { }
    //}
}

namespace Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users.Guid
{
    public abstract class User<TUsername, TRefreshToken> : User<UserIdentifierValue, System.Guid, TUsername, TRefreshToken, RefreshTokenIdentifierValue, System.Guid>
        where TUsername : IUsernameValue
        where TRefreshToken : IRefreshToken<RefreshTokenIdentifierValue, System.Guid, UserIdentifierValue, System.Guid>, new()

    {
        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins, numberOfInactiveTokensToStore) { }
    }

    public abstract class User<TRefreshToken> : User<UserIdentifierValue, System.Guid, EmailUsernameValue, TRefreshToken, RefreshTokenIdentifierValue, System.Guid>
            where TRefreshToken : IRefreshToken<RefreshTokenIdentifierValue, System.Guid, UserIdentifierValue, System.Guid>, new()
    {
        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins, numberOfInactiveTokensToStore) { }
    }

    public abstract class User : User<UserIdentifierValue, System.Guid, EmailUsernameValue, RefreshTokens.Guid.RefreshToken, RefreshTokenIdentifierValue, System.Guid>
    {
        protected User(int maxFailedLogins, int numberOfInactiveTokensToStore) : base(maxFailedLogins, numberOfInactiveTokensToStore) { }
    }
}
