namespace Fanzoo.Kernel
{
    public static partial class Errors
    {
        public static class ValueObjects
        {
            public static class StringValue
            {
                public static Error InvalidFormat => new("invalid.string.format", "Invalid string format.");
            }

            public static class EmailValue
            {
                public static Error InvalidFormat => new("invalid.email.format", "Invalid email format.");
            }

            public static class UrlValue
            {
                public static Error InvalidFormat => new("invalid.url.format", "Invalid url format.");
            }

            public static class PhoneValue
            {
                public static Error InvalidFormat => new("invalid.phone.format", "Invalid phone format.");
            }

            public static class UsernameValue
            {
                public static Error InvalidFormat => new("invalid.username.format", "Invalid username format.");
            }

            public static class GuidIdentifierValue
            {
                public static Error GuidCannotBeEmpty => new("guid.cannot.be.empty", "Guid cannot be empty.");
            }

            public static class PasswordValue
            {
                public static Error InvalidFormat => new("invalid.password.format", "Invalid password format.");
            }

            public static class HashedPasswordValue
            {
                public static Error InvalidFormat => new("invalid.password.hash.format", "Invalid password hash format.");
            }

            public static class AddressValue
            {
                public static Error InvalidFormat => new("invalid.address.format", "Invalid address format.");
            }

            public static class NameValue
            {
                public static Error InvalidFormat => new("invalid.name.format", "Invalid name format.");
            }

            public static class PostalCodeValue
            {
                public static Error InvalidFormat => new("invalid.postalcode.format", "Invalid postal code format.");
            }

            public static class RegionValue
            {
                public static Error InvalidRegionCode => new("not.a.valid.region.code", "Not a valid region abbreviation.");
            }

            public static class MoneyValue
            {
                public static Error GreaterThanOrEqualToZero => new("greater.than.or.equal.to.zero", "Amount must be greater than or equal to zero.");

                public static Error InvalidNumberOfDecimalPlaces => new("invalid.number.of.decimal.places", "The minor units (decimal places) for this currency is invalid.");

                public static Error CannotPerformArithmeticOperationOnDifferentCurrencies => new("cannot.perform.arithmetic.operation.on.different.currencies", "Cannot perform arithmetic operation on different currencies.");
            }

            public static class RefreshTokenValue
            {
                public static Error InvalidFormat => new("invalid.refreshtoken.format", "Invalid refresh token format.");
            }

            public static class IPAddressValue
            {
                public static Error InvalidFormat => new("invalid.ipaddress.format", "Invalid IP address format.");
            }

            public static class StripePaymentIntentIdentifier
            {
                public static Error InvalidFormat => new("invalid.stripe.paymentintentidentifier.format", "Invalid Stripe PaymentIntent identifier format.");
            }

            public static class StripeCustomerIdentifier
            {
                public static Error InvalidFormat => new("invalid.stripe.customeridentifier.format", "Invalid Stripe Customer identifier format.");
            }
        }
    }
}
