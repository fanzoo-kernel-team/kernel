#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

using System.Globalization;
using System.Reflection;

namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class DefaultStringValue<TImplementor> : StringValue where TImplementor : DefaultStringValue<TImplementor>
    {
        private const int DEFAULT_MAX_STRING_LENGTH = 255;

        protected DefaultStringValue() : base() { } //ORM

        protected DefaultStringValue(string value, int maxSize) : base(value, maxSize) { }

        public static ValueResult<TImplementor, Error> Create(string value)
        {
            var isValid = Check.For
                .NullOrWhiteSpace(value)
                .And
                .LengthExceedsMaximum(value, DEFAULT_MAX_STRING_LENGTH)
                    .IsValid;

            return isValid ?
                (TImplementor)Activator.CreateInstance(
                    typeof(TImplementor),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new object[] { value, DEFAULT_MAX_STRING_LENGTH },
                    CultureInfo.InvariantCulture)!
                : Errors.ValueObjects.DefaultStringValue.InvalidFormat;
        }
    }
}
