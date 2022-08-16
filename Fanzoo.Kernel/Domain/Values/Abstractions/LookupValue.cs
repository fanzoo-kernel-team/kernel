using System.Reflection;

namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class LookupValue<TInheritor, TPrimitive> : ValueObject
        where TInheritor : LookupValue<TInheritor, TPrimitive>
        where TPrimitive : notnull
    {
        protected LookupValue() { } //ORM

        protected LookupValue(TPrimitive id)
        {
            Id = id;
        }

        public TPrimitive Id { get; init; } = default!;

        public static TInheritor Find(TPrimitive id) => typeof(TInheritor)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(TInheritor))
                        .Select(f => f.GetValue(null))
                            .Cast<LookupValue<TInheritor, TPrimitive>>()
                                    .Cast<TInheritor>()
                                        .Single(f => f.Id.Equals(id));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
