namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class IdentifierValue<TPrimitive>(TPrimitive id) : SimpleValueObject<TPrimitive>(id) where TPrimitive : notnull, new()
    {
        protected IdentifierValue() : this(new()) { }
    }
}
