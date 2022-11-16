namespace Fanzoo.Kernel.Domain.Values
{
    public abstract class ValueObject : IComparable, IComparable<ValueObject>
    {
        private int? _hashCode;

        private IEnumerable<object> _equalityValues = Array.Empty<object>();

        internal static Type GetUnproxiedType(object obj)
        {
            const string EFCoreProxyPrefix = "Castle.Proxies.";
            const string NHibernateProxyPostfix = "Proxy";

            var type = obj.GetType();

            var name = type.ToString();

            return name.Contains(EFCoreProxyPrefix) || name.Contains(NHibernateProxyPostfix)
                ? type.BaseType ?? throw new InvalidOperationException("Proxy type has no base type.")
                : type;
        }

        protected abstract IEnumerable<object> GetEqualityValues();

        private IEnumerable<object> EqualityValues
        {
            get
            {
                if (!_equalityValues.Any())
                {
                    _equalityValues = GetEqualityValues();
                }

                return _equalityValues;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "Analyzer incorrectly identifying immutable values.")]
        public override int GetHashCode()
        {
            if (_hashCode is null)
            {
                var hash = new HashCode();

                foreach (var value in EqualityValues)
                {
                    hash.Add(value);
                }

                _hashCode = hash.ToHashCode();
            }

            return _hashCode.Value;
        }

        public override bool Equals(object? obj) =>
            (obj != null)
                && GetUnproxiedType(this) == GetUnproxiedType(obj)
                && EqualityValues.SequenceEqual(((ValueObject)obj).EqualityValues);

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            var left = GetUnproxiedType(this);
            var right = GetUnproxiedType(obj);

            if (left != right)
            {
                return string.Compare(left.ToString(), right.ToString(), StringComparison.Ordinal);
            }

            var other = (ValueObject)obj;

            var values = EqualityValues.ToArray();
            var otherValues = other.EqualityValues.ToArray();

            for (var i = 0; i < values.Length; i++)
            {
                var result = Compare(values[i], otherValues[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        public int CompareTo(ValueObject? other) => CompareTo(other as object);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Readability")]
        private static int Compare(object left, object right)
        {
            if (left is null && right is null)
            {
                return 0;
            }

            if (left is null)
            {
                return -1;
            }

            if (right is null)
            {
                return 1;
            }

            if (left is IComparable comparableLeft && right is IComparable comparableRight)
            {
                return comparableLeft.CompareTo(comparableRight);
            }

            return left.Equals(right) ? 0 : -1;
        }

        public static bool operator ==(ValueObject left, ValueObject right) => left is null ? right is null : left.Equals(right);

        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);

        public static bool operator <(ValueObject left, ValueObject right) => left is null ? right is not null : left.CompareTo(right) < 0;

        public static bool operator <=(ValueObject left, ValueObject right) => left is null || left.CompareTo(right) <= 0;

        public static bool operator >(ValueObject left, ValueObject right) => left is not null && left.CompareTo(right) > 0;

        public static bool operator >=(ValueObject left, ValueObject right) => left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
