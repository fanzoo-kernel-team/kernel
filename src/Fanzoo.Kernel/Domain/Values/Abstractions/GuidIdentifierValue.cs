namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class GuidIdentifierValue<TInheritor>(Guid id) : IdentifierValue<Guid>(id) where TInheritor : GuidIdentifierValue<TInheritor>, new()
    {
        public static ValueResult<TInheritor, Error> Create(Guid id) =>
            Check.For.IsNotEmpty(id) ? new TInheritor() { Value = id } : Errors.ValueObjects.GuidIdentifierValue.GuidCannotBeEmpty;

        protected GuidIdentifierValue() : this(Guid.NewGuid()) { }
    }
}
