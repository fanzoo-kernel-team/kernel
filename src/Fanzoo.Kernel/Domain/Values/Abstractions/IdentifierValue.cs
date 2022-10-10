namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class IdentifierValue<TPrimitive> : SimpleValueObject<TPrimitive> where TPrimitive : notnull, new()
    {
        protected IdentifierValue() : this(new()) { }

        protected IdentifierValue(TPrimitive id) : base(id) { }
    }
}
