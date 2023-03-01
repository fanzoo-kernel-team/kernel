namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class PhoneValue : RequiredStringValue
    {
        private PhoneValue() { } //ORM

        public PhoneValue(string value) : base(value)
        {
            //sanitize
            var digits = value
                .ToDigits()
                    .TrimStart('1');

            Guard.Against.NonMatchingRegex(digits, PhonePattern, nameof(value));

        }

        public static ValueResult<PhoneValue, Error> Create(string phone)
        {
            //sanitize
            var digits = phone
                .ToDigits()
                    .TrimStart('1');

            return Check.For.IsValidPhoneFormat(digits)
                ? new PhoneValue(digits)
                : Errors.ValueObjects.PhoneValue.InvalidFormat;
        }

        public override string ToString() => Value.Format("{0:(###) ###-####}");

        public static implicit operator PhoneValue(string value) => new(value);

        private const string PhonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
    }
}
