namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class LongStringValue(string value) : StringValue(value, DatabaseCatalog.FieldLength.Long)
    {
        public static ValueResult<LongStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNull(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Long);

            return isValid ? new LongStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator LongStringValue(string value) => new(value);
    }

    public sealed class LongRequiredStringValue(string value) : RequiredStringValue(value, DatabaseCatalog.FieldLength.Long)
    {
        public static ValueResult<LongRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Long);

            return isValid ? new LongRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator LongRequiredStringValue(string value) => new(value);
    }
}
