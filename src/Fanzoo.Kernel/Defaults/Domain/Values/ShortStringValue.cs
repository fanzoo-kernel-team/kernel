namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class ShortStringValue : StringValue
    {
        public ShortStringValue(string value) : base(value, DatabaseCatalog.FieldLength.Short) { }

        public static ValueResult<ShortStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .Null(value)
                .And
                .LengthExceedsMaximum(value, DatabaseCatalog.FieldLength.Short)
                    .IsValid;

            return isValid ? new ShortStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator ShortStringValue(string value) => new(value);
    }

    public sealed class ShortRequiredStringValue : RequiredStringValue
    {
        public ShortRequiredStringValue(string value) : base(value, DatabaseCatalog.FieldLength.Short) { }

        public static ValueResult<ShortRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For
                .NullOrWhiteSpace(value)
                .And
                .LengthExceedsMaximum(value, DatabaseCatalog.FieldLength.Short)
                    .IsValid;

            return isValid ? new ShortRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator ShortRequiredStringValue(string value) => new(value);
    }
}
