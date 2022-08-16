namespace Fanzoo.Kernel.Data.Mapping
{
    public abstract class ImmutableEntityClassMap<TEntity, TIdentifier, TPrimitive> : EntityClassMap<TEntity, TIdentifier, TPrimitive>
        where TEntity : class, IEntity<TIdentifier, TPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {
        protected ImmutableEntityClassMap() : base()
        {
            Map(p => CreatedDate, nameof(CreatedDate))
                .Access
                    .NoOp();

            Map(p => CreatedBy, nameof(CreatedBy))
                .Access
                    .NoOp();
        }

        public DateTime CreatedDate { get; set; } = default!;

        public string CreatedBy { get; set; } = default!;

    }
}
