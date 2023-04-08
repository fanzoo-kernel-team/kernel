namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class USPostalCodeValue : RequiredStringValue
    {
        private USPostalCodeValue() { } //ORM

        public USPostalCodeValue(string postalCode) : base(postalCode)
        {
            Guard.Against.NullOrWhiteSpace(postalCode, nameof(postalCode));
            Guard.Against.InvalidUSPostalCode(postalCode, nameof(postalCode));
        }

        public static ValueResult<USPostalCodeValue, Error> Create(string postalCode) => CanCreate(postalCode) ? new USPostalCodeValue(postalCode) : Errors.ValueObjects.PostalCodeValue.InvalidFormat;

        public static implicit operator USPostalCodeValue(string value) => new(value);

        public static bool CanCreate(string postalCode) => Check.For.IsValidUSPotalCode(postalCode);

    }
}
