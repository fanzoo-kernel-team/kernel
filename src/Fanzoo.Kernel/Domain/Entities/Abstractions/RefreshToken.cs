namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IRefreshToken<out TIdentifier, TPrimitive> : IMutableEntity
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()

    {
        TIdentifier Id { get; }

        RefreshTokenValue Token { get; }

        DateTime Issued { get; }

        DateTime ExpirationDate { get; }

        IPAddressValue IPAddress { get; }

        bool IsRevoked { get; }

        DateTime? Revoked { get; }

        bool IsExpired { get; }

        bool IsActive { get; }

        void Revoke();
    }

    public abstract class RefreshToken<TIdentifier, TPrimitive> : Entity<TIdentifier, TPrimitive>, IRefreshToken<TIdentifier, TPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
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
