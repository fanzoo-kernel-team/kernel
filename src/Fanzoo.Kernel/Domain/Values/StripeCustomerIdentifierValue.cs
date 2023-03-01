namespace Fanzoo.Kernel.Domain.Values.Stripe
{
    public sealed class StripeCustomerIdentifierValue : RequiredStringValue
    {
        private StripeCustomerIdentifierValue() { } //ORM

        public StripeCustomerIdentifierValue(string value) : base(value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));
            Guard.Against.InvalidPrefix(value, "cus_", nameof(value));
        }

        public static ValueResult<StripeCustomerIdentifierValue, Error> Create(string stripeCustomerIdentifier)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(stripeCustomerIdentifier)
                .And
                .StartsWith(stripeCustomerIdentifier, "cus_");

            return isValid ? new StripeCustomerIdentifierValue(stripeCustomerIdentifier) : Errors.ValueObjects.StripeCustomerIdentifier.InvalidFormat;

        }
    }
}
