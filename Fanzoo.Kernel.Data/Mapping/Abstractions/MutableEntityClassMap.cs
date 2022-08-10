namespace Fanzoo.Kernel.Data.Mapping
{
    public abstract class MutableEntityClassMap<TEntity, TIdentifier, TPrimitive> : ImmutableEntityClassMap<TEntity, TIdentifier, TPrimitive>
        where TEntity : class, IEntity<TIdentifier, TPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {
        protected MutableEntityClassMap() : base()
        {
            Map(p => UpdatedDate, nameof(UpdatedDate))
                .Access
                    .NoOp();

            Map(p => UpdatedBy, nameof(UpdatedBy))
                .Access
                    .NoOp();
        }

        public DateTime UpdatedDate { get; set; } = default!;

        public string UpdatedBy { get; set; } = default!;

    }
}
