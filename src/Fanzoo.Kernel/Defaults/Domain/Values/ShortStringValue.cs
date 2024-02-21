namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class ShortStringValue(string value) : StringValue(value, DatabaseCatalog.FieldLength.Short)
    {
        public static ValueResult<ShortStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNull(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Short);

            return isValid ? new ShortStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator ShortStringValue(string value) => new(value);
    }

    public sealed class ShortRequiredStringValue(string value) : RequiredStringValue(value, DatabaseCatalog.FieldLength.Short)
    {
        public static ValueResult<ShortRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(value)
                .And
                .LengthIsLessThanOrEqual(value, DatabaseCatalog.FieldLength.Short);

            return isValid ? new ShortRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator ShortRequiredStringValue(string value) => new(value);
    }
}
