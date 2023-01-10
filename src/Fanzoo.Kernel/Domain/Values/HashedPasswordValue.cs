namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class HashedPasswordValue : RequiredStringValue
    {
        private HashedPasswordValue() { } //ORM

        public HashedPasswordValue(string value) : base(value)
        {
            Guard.Against.InvalidBase64String(value, nameof(value));
        }

        public static ValueResult<HashedPasswordValue, Error> Create(string hashedPassword) =>
            Check.For.Base64String(hashedPassword).IsValid
                ? new HashedPasswordValue(hashedPassword)
                : Errors.ValueObjects.HashedPasswordValue.InvalidFormat;

        public static implicit operator HashedPasswordValue(string value) => new(value);

    }
}
