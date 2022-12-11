namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IRefreshToken<TIdentifier, TPrimitive> : IMutableEntity
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()

    {
        public TIdentifier Id { get; }

        public RefreshTokenValue Token { get; }

        public DateTime Issued { get; }

        public DateTime ExpirationDate { get; }

        public IPAddressValue IPAddress { get; }

        public bool IsRevoked { get; }

        public DateTime? Revoked { get; }

        bool IsExpired { get; }

        bool IsActive { get; }

        void Revoke();
    }
}
