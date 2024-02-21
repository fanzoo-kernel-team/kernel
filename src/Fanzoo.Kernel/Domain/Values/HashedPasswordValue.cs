namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class HashedPasswordValue : RequiredStringValue
    {
        private HashedPasswordValue() { } //ORM

        public HashedPasswordValue(string value) : base(value) => Guard.Against.InvalidBase64String(value, nameof(value));

        public static ValueResult<HashedPasswordValue, Error> Create(string hashedPassword) =>
            CanCreate(hashedPassword)
                ? new HashedPasswordValue(hashedPassword)
                : Errors.ValueObjects.HashedPasswordValue.InvalidFormat;

        public static implicit operator HashedPasswordValue(string value) => new(value);

        public static bool CanCreate(string hashedPassword) => Check.For.IsBase64String(hashedPassword);

    }
}
