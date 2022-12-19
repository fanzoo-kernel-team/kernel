#pragma warning disable S3875 // "operator==" should not be overloaded on reference types
namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IImmutableEntity { }

    public interface IMutableEntity : IImmutableEntity { }

    public interface ITrackableEntity
    {
        bool IsTransient { get; }

        void SetAsLoadedOrSaved();
    }

    public interface IEntity<out TIdentifier, TPrimitive>
    where TIdentifier : notnull, IdentifierValue<TPrimitive>
    where TPrimitive : notnull, new()
    {
        TIdentifier Id { get; }
    }

    public abstract class Entity<TIdentifier, TPrimitive> : IEntity<TIdentifier, TPrimitive>, ITrackableEntity
        where TIdentifier : IdentifierValue<TPrimitive>, new()
        where TPrimitive : notnull, new()
    {
        private bool _isTransient = true;

        public virtual TIdentifier Id { get; init; } = new();

        bool ITrackableEntity.IsTransient => _isTransient;

        public override string ToString() => GetType().Name + ": " + Id.ToString();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Makes the code less readable.")]
        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TIdentifier, TPrimitive> other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetUnproxiedType(this) != GetUnproxiedType(other))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TIdentifier, TPrimitive> a, Entity<TIdentifier, TPrimitive> b) => !((a is null && b is null) || a is null || b is null) && a.Equals(b);

        public static bool operator !=(Entity<TIdentifier, TPrimitive> a, Entity<TIdentifier, TPrimitive> b) => !(a == b);

        public override int GetHashCode() => (GetUnproxiedType(this).ToString() + Id).GetHashCode();

        private static Type GetUnproxiedType(object o)
        {
            const string EFCoreProxyPrefix = "Castle.Proxies.";
            const string NHibernateProxyPostfix = "Proxy";

            var type = o.GetType();

            var typeString = type.ToString();

            return typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix)
                ? type.BaseType ?? throw new NullReferenceException("Base type not found.")
                : type;
        }

        void ITrackableEntity.SetAsLoadedOrSaved() => _isTransient = false;
    }
}
