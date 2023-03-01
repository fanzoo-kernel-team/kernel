namespace Fanzoo.Kernel.Domain.Values.Stripe
{
    public sealed class StripePaymentIntentIdentifierValue : RequiredStringValue
    {
        private StripePaymentIntentIdentifierValue() { } //ORM

        public StripePaymentIntentIdentifierValue(string value) : base(value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));
            Guard.Against.InvalidPrefix(value, "pi_", nameof(value));
        }

        public static ValueResult<StripePaymentIntentIdentifierValue, Error> Create(string stripePaymentIntentIdentifier)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(stripePaymentIntentIdentifier)
                .And
                .StartsWith(stripePaymentIntentIdentifier, "pi_");

            return isValid ? new StripePaymentIntentIdentifierValue(stripePaymentIntentIdentifier) : Errors.ValueObjects.StripePaymentIntentIdentifier.InvalidFormat;

        }
    }
}
