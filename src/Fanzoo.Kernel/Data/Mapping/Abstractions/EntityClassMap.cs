using System.Linq.Expressions;

namespace Fanzoo.Kernel.Data.Mapping
{
    public abstract class EntityClassMap<TEntity, TIdentifier, TPrimitive> : ClassMap<TEntity>
        where TEntity : class, IEntity<TIdentifier, TPrimitive>
        where TIdentifier : IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {
        protected EntityClassMap()
        {
            CompositeId(e => e.Id)
                .KeyProperty(p => p.Value, "Id");
        }

        protected ComponentPart<TComponent?> MapValueObject<TComponent>(Expression<Func<TEntity, TComponent?>> entityFieldExpression, string memberName, string columnName, bool nullable = false)
            where TComponent : ValueObject =>
              ClassMapExtensions.MapValueObject(this, entityFieldExpression, memberName, columnName, nullable);

        protected ComponentPart<TComponent?> MapValueObject<TComponent>(Expression<Func<TEntity, TComponent?>> entityFieldExpression, string columnName, bool nullable = false)
            where TComponent : ValueObject =>
                ClassMapExtensions.MapValueObject(this, entityFieldExpression, columnName, nullable);

        protected ComponentPart<TComponent?> MapValueObject<TComponent>(Expression<Func<TEntity, TComponent?>> entityFieldExpression, bool nullable = false)
            where TComponent : ValueObject =>
                ClassMapExtensions.MapValueObject(this, entityFieldExpression, nullable);

        protected ComponentPart<AddressValue?> MapAddressValue(Expression<Func<TEntity, AddressValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false) =>
            ClassMapExtensions.MapAddressValue(this, entityFieldExpression, columnPrefix, nullable);

        protected ComponentPart<MoneyValue?> MapMoneyValue(Expression<Func<TEntity, MoneyValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false) =>
            ClassMapExtensions.MapMoneyValue(this, entityFieldExpression, columnPrefix, nullable);

        protected ComponentPart<NameValue?> MapNameValue(Expression<Func<TEntity, NameValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false) =>
            ClassMapExtensions.MapNameValue(this, entityFieldExpression, columnPrefix, nullable);

        protected OneToManyPart<TChild> HasMany<TChild>(Expression<Func<TEntity, IEnumerable<TChild>>> memberExpression, string keyColumn) =>
            ClassMapExtensions.HasMany(this, memberExpression, keyColumn);
    }
}
