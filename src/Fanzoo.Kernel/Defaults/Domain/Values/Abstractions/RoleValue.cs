namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public abstract class RoleValue<TInheritor> : NamedLookupValue<TInheritor, Guid>, IRoleValue<Guid>
        where TInheritor : NamedLookupValue<TInheritor, Guid>
    {
        protected RoleValue() { } //ORM

        protected RoleValue(Guid id, string name) : base(id, name) { }
    }
}
