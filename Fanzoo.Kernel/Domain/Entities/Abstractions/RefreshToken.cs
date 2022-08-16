namespace Fanzoo.Kernel.Domain.Entities
{
    public abstract class RefreshToken<TIdentifier, TPrimitive, TUserIdentifier, TUserPrimitive> : Entity<TIdentifier, TPrimitive>, IRefreshToken<TIdentifier, TPrimitive, TUserIdentifier, TUserPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
        where TUserIdentifier : IdentifierValue<TUserPrimitive>
        where TUserPrimitive : notnull, new()
    {
        public RefreshTokenValue Token { get; protected set; } = default!;

        public DateTime Issued { get; protected set; } = default!;

        public DateTime ExpirationDate { get; protected set; } = default!;

        public IPAddressValue IPAddress { get; protected set; } = default!;

        public DateTime? Revoked { get; protected set; }

        public bool IsRevoked => Revoked is not null;

        public bool IsActive => !IsRevoked && !IsExpired;

        public bool IsExpired => ExpirationDate <= SystemDateTime.Now;

        public virtual void Revoke() => Revoked = SystemDateTime.Now;
    }
}
