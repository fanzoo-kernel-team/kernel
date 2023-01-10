using System.Text;

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class PasswordValue : RequiredStringValue, IPasswordValue
    {
        private const int MinimumPasswordLength = 6;

        private const int MaximumPasswordLength = 50;

        private PasswordValue() { } //ORM

        public PasswordValue(string password) : base(password)
        {
            Guard.Against.LengthLessThan(password, MinimumPasswordLength, nameof(password));
            Guard.Against.LengthGreaterThan(password, MaximumPasswordLength, nameof(password));
        }

        public static ValueResult<PasswordValue, Error> Create(string password)
        {
            password = password.Trim();

            var isValid = Check.For
                .LengthLessThanMinimum(password, MinimumPasswordLength)
                .And
                .LengthExceedsMaximum(password, MaximumPasswordLength)
                    .IsValid;

            return isValid ? new PasswordValue(password) : Errors.ValueObjects.PasswordValue.InvalidFormat;

        }

        public static PasswordValue Generate()
        {
            const string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var sb = new StringBuilder();

            var random = new Random();

            while (sb.Length < MinimumPasswordLength)
            {
                var character = characters.Substring(random.Next(0, characters.Length), 1);
                sb.Append(character);
            }

            return new PasswordValue(sb.ToString());
        }

        public static implicit operator PasswordValue(string s) => new(s);
    }
}
