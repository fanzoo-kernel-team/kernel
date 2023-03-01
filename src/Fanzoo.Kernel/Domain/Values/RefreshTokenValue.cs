using System.Security.Cryptography;

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class RefreshTokenValue : RequiredStringValue
    {
        private RefreshTokenValue() { } //ORM

        public RefreshTokenValue(string refreshToken) : base(refreshToken)
        {
            Guard.Against.NullOrWhiteSpace(refreshToken, nameof(refreshToken));
            Guard.Against.InvalidBase64String(refreshToken, nameof(refreshToken));
        }

        public static ValueResult<RefreshTokenValue, Error> Create(string refreshToken)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(refreshToken)
                .And
                .IsBase64String(refreshToken);

            return isValid
                ? new RefreshTokenValue(refreshToken)
                : Errors.ValueObjects.RefreshTokenValue.InvalidFormat;
        }

        public static RefreshTokenValue Generate()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128));

            return new RefreshTokenValue(token);
        }

        public static implicit operator RefreshTokenValue(string value) => new(value);

    }
}
