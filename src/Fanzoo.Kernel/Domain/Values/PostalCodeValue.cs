namespace Fanzoo.Kernel.Domain.Values
{
    //TODO: support different regions
    public sealed class PostalCodeValue : StringValue
    {
        private PostalCodeValue() { } //ORM

        public PostalCodeValue(string postalCode) : base(postalCode)
        {
            Guard.Against.NullOrWhiteSpace(postalCode, nameof(postalCode));
            Guard.Against.NonMatchingRegex(postalCode, "^\\b\\d{5}\\b(?:[- ]{1}\\d{4})?$", nameof(postalCode));
        }

        public static Result<PostalCodeValue, Error> Create(string postalCode)
        {
            var isValid = Check.For
                .NullOrWhiteSpace(postalCode)
                .And
                .NonMatchingRegex(postalCode, "^\\b\\d{5}\\b(?:[- ]{1}\\d{4})?$")
                    .IsValid;

            return isValid ? new PostalCodeValue(postalCode) : Errors.ValueObjects.PostalCodeValue.InvalidFormat;
        }
    }
}
