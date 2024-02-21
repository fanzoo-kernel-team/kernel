namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class DefaultStringValue(string value) : StringValue(value, DatabaseCatalog.FieldLength.Default)
    {
        public static ValueResult<DefaultStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNull(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Default);

            return isValid ? new DefaultStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator DefaultStringValue(string value) => new(value);
    }

    public sealed class DefaultRequiredStringValue(string value) : RequiredStringValue(value, DatabaseCatalog.FieldLength.Default)
    {
        public static ValueResult<DefaultRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Default);

            return isValid ? new DefaultRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator DefaultRequiredStringValue(string value) => new(value);
    }
}
