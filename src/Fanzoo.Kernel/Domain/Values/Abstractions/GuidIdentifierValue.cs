namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class GuidIdentifierValue<TInheritor> : IdentifierValue<Guid> where TInheritor : GuidIdentifierValue<TInheritor>, new()
    {
        public static Result<TInheritor, Error> Create(Guid id) =>
            Check.For.Empty(id).IsValid ? new TInheritor() { Value = id } : Errors.ValueObjects.GuidIdentifierValue.GuidCannotBeEmpty;

        protected GuidIdentifierValue() : this(Guid.NewGuid()) { }

        private GuidIdentifierValue(Guid id) : base(id) { }
    }
}
