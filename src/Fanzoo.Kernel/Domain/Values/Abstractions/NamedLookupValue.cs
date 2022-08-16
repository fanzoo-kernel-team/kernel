using System.Reflection;

namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class NamedLookupValue<TInheritor, TPrimitive> : LookupValue<TInheritor, TPrimitive>
        where TInheritor : NamedLookupValue<TInheritor, TPrimitive>
        where TPrimitive : notnull
    {
        private bool _initialized = false;

        private string _name = default!;

        protected NamedLookupValue() { } //ORM

        protected NamedLookupValue(TPrimitive id, string name) : base(id)
        {
            Name = name;
        }

        public string Name
        {
            get
            {
                if (!_initialized)
                {
                    _name = Find(Id).Name;
                    _initialized = true;
                }

                return _name;
            }

            set
            {
                _name = value;
                _initialized = true;
            }
        }

        public static TInheritor Find(string name) => typeof(TInheritor)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(TInheritor))
                    .Select(f => f.GetValue(null))
                        .Cast<NamedLookupValue<TInheritor, TPrimitive>>()
                                .Cast<TInheritor>()
                                    .Single(f => f.Name.Equals(name));

    }
}
