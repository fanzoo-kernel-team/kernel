using Fanzoo.Kernel.Domain.Entities.Users;

namespace Fanzoo.Kernel.Domain.Entities.Users
{
    public interface IUser<TIdentifier, TPrimitive, out TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
    {
        public TIdentifier Id { get; }

        public TUsername Username { get; }

        public EmailValue Email { get; }

        public HashedPasswordValue Password { get; }

        public bool IsLockedOut { get; }

        public bool IsActive { get; }

        public DateTime LastAuthenticationChange { get; }

        public void RecordValidLogin();

        public void RecordInvalidLogin();

        public void SignOut();

        public void Lock();

        public void Unlock();

        public void UpdatePassword(HashedPasswordValue password);

        public bool RequiresAuthentication(DateTime persistedLastAuthenticationChange);
    }
}

namespace Fanzoo.Kernel.Domain.Entities.RefreshTokens.Users
{
    public interface IUser<TIdentifier, TPrimitive, TUsername, TRefreshToken, TTokenIdentifier, TTokenPrimitive> : IUser<TIdentifier, TPrimitive, TUsername>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
        where TUsername : IUsernameValue
        where TRefreshToken : IRefreshToken<TTokenIdentifier, TTokenPrimitive, TIdentifier, TPrimitive>
        where TTokenIdentifier : IdentifierValue<TTokenPrimitive>
        where TTokenPrimitive : notnull, new()
    {
        public IEnumerable<TRefreshToken> RefreshTokens { get; }

        public TRefreshToken AddRefreshToken(DateTime expirationDate, IPAddressValue ipAddress);

        void RevokeAllTokens();

        void RevokeToken(RefreshTokenValue token);
    }
}