namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class RequiredStringValue : StringValue
    {
        protected RequiredStringValue() : base(string.Empty) { }

        protected RequiredStringValue(string value) : this(value, int.MaxValue) { }

        protected RequiredStringValue(string value, int maxSize) : base(value, maxSize) => Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
}
