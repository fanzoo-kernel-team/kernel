namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class LongStringValue : StringValue
    {
        public LongStringValue(string value) : base(value, DatabaseCatalog.FieldLength.Long) { }

        public static ValueResult<LongStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .Null(value)
                .And
                .LengthExceedsMaximum(value, DatabaseCatalog.FieldLength.Long)
                    .IsValid;

            return isValid ? new LongStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator LongStringValue(string value) => new(value);
    }

    public sealed class LongRequiredStringValue : RequiredStringValue
    {
        public LongRequiredStringValue(string value) : base(value, DatabaseCatalog.FieldLength.Long) { }

        public static ValueResult<LongRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NullOrWhiteSpace(value)
                .And
                .LengthExceedsMaximum(value, DatabaseCatalog.FieldLength.Long)
                    .IsValid;

            return isValid ? new LongRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator LongRequiredStringValue(string value) => new(value);
    }
}
