using System.Linq.Expressions;
using FluentNHibernate;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public static class ClassMapExtensions
    {
        public static PropertyPart Nullable(this PropertyPart propertyPart, bool nullable = true) => nullable ? propertyPart.Nullable() : propertyPart.Not.Nullable();

        public static ComponentPart<TComponent?> MapValueObject<TEntity, TComponent>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, TComponent?>> entityFieldExpression, string memberName, string columnName, bool nullable = false)
            where TComponent : ValueObject =>
                map.Component(entityFieldExpression, x => x.Map(Reveal.Member<TComponent?>(memberName), columnName).Nullable(nullable));

        public static ComponentPart<TComponent?> MapValueObject<TEntity, TComponent>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, TComponent?>> entityFieldExpression, string columnName, bool nullable = false)
            where TComponent : ValueObject =>
                map.MapValueObject(entityFieldExpression, "Value", columnName, nullable);

        public static ComponentPart<TComponent?> MapValueObject<TEntity, TComponent>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, TComponent?>> entityFieldExpression, bool nullable = false)
            where TComponent : ValueObject =>
                map.MapValueObject(entityFieldExpression, entityFieldExpression.ToMember().Name, nullable);

        public static ComponentPart<AddressValue?> MapAddressValue<TEntity>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, AddressValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false)
        {
            var mapping = map.Component(entityFieldExpression, x =>
            {
                x.Map(a => a!.PrimaryAddress).Nullable(nullable);
                x.Map(a => a!.SecondaryAddress).Nullable(true);
                x.Map(a => a!.City).Nullable(nullable);
                x.MapValueObject(a => a!.Region, nullable);
                x.MapValueObject(a => a!.PostalCode, nullable);
            });

            mapping.ColumnPrefix(columnPrefix);

            return mapping;
        }

        public static ComponentPart<MoneyValue?> MapMoneyValue<TEntity>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, MoneyValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false)
        {
            var mapping = map.Component(entityFieldExpression, x =>
            {
                x.Map(m => m!.Amount, "Amount").Nullable(nullable);
                x.Component(m => m!.Currency, x => x.Map(Reveal.Member<CurrencyValue>("Code"), "Currency").Nullable(nullable));
            });

            mapping.ColumnPrefix(columnPrefix);

            return mapping;
        }

        public static ComponentPart<NameValue?> MapNameValue<TEntity>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, NameValue?>> entityFieldExpression, string columnPrefix = "", bool nullable = false)
        {
            var mapping = map.Component(entityFieldExpression, x =>
            {
                x.Map(n => n!.FirstName, "FirstName").Nullable(nullable);
                x.Map(n => n!.LastName, "LastName").Nullable(nullable);
            });

            mapping.ColumnPrefix(columnPrefix);

            return mapping;
        }

        public static OneToManyPart<TChild> HasMany<TEntity, TChild>(this ClasslikeMapBase<TEntity> map, Expression<Func<TEntity, IEnumerable<TChild>>> memberExpression, string keyColumn) =>
            map.HasMany(memberExpression)
                .KeyColumn(keyColumn)
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Not.Inverse()
                .Access
                    .CamelCaseField(Prefix.Underscore)
                .Cascade
                    .AllDeleteOrphan();
    }
}
