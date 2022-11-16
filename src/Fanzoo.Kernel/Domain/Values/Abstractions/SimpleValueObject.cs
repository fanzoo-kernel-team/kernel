namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class SimpleValueObject<T> : ValueObject where T : notnull
    {
        protected SimpleValueObject(T value)
        {
            Guard.Against.Null(value, nameof(value));

            Value = value;
        }

        public T Value { get; init; }

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return Value;
        }

        public static implicit operator T(SimpleValueObject<T> valueObject) => valueObject.Value;

        public override string ToString() => Value.ToString()!; //for some reason ToString doesn't understand the notnull constraint

    }
}
