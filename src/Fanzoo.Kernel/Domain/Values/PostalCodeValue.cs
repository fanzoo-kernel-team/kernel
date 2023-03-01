namespace Fanzoo.Kernel.Domain.Values
{
    //TODO: support different regions
    public sealed class PostalCodeValue : RequiredStringValue
    {
        private PostalCodeValue() { } //ORM

        public PostalCodeValue(string postalCode) : base(postalCode)
        {
            Guard.Against.NullOrWhiteSpace(postalCode, nameof(postalCode));
            Guard.Against.NonMatchingRegex(postalCode, "^\\b\\d{5}\\b(?:[- ]{1}\\d{4})?$", nameof(postalCode));
        }

        public static ValueResult<PostalCodeValue, Error> Create(string postalCode)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(postalCode)
                .And
                .Matches(postalCode, "^\\b\\d{5}\\b(?:[- ]{1}\\d{4})?$");

            return isValid ? new PostalCodeValue(postalCode) : Errors.ValueObjects.PostalCodeValue.InvalidFormat;
        }

        public static implicit operator PostalCodeValue(string value) => new(value);

    }
}
