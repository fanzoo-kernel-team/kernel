namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class IdentifierValue<TPrimitive> : ValueObject where TPrimitive : notnull, new()
    {
        protected IdentifierValue() : this(new()) { }

        protected IdentifierValue(TPrimitive id)
        {
            Guard.Against.Null(id, nameof(id));

            Value = id;
        }

        public TPrimitive Value { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString()!; //compiler is weird here about null-ability

    }
}
