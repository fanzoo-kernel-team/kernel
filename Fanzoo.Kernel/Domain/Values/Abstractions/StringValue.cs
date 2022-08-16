namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class StringValue : SimpleValueObject<string>
    {
        protected StringValue() : base(string.Empty) { }

        protected StringValue(string value) : this(value, int.MaxValue) { }

        protected StringValue(string value, int maxSize) : base(value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            Guard.Against.ExceedsMaxValue(value.Length, maxSize, nameof(value));
        }
    }
}
