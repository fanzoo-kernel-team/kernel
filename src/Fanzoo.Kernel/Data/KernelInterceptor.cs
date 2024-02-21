using NHibernate.Type;
using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Data
{
    public class KernelInterceptor(IServiceProvider serviceProvider) : EmptyInterceptor
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private ISession? _session;

        private static readonly string[] _propertiesToIgnore = ["CreatedDate", "CreatedBy", "UpdatedDate", "UpdatedBy"];

        public T? GetService<T>() => (T?)_serviceProvider.GetService(typeof(T));

        public override void SetSession(ISession session) => _session = session;

        public override int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            var session = _session?.GetSessionImplementation();

            if (session is null)
            {
                return [];
            }

            var entry = session.PersistenceContext.GetEntry(entity);

            if (entry is null)
            {
                return [];
            }

            var dirtyIndexes =
                session
                    .Factory
                        .GetEntityPersister(entry.EntityName)
                            .FindDirty(currentState, previousState, entity, session);

            if (dirtyIndexes is null)
            {
                return [];
            }

            var dirtyProperties = new List<int>(dirtyIndexes);

            for (var i = 0; i < propertyNames.Length; i++)
            {
                if (_propertiesToIgnore.Contains(propertyNames[i]))
                {
                    dirtyProperties.Remove(i);
                }
            }

            return [.. dirtyProperties];
        }

        public override bool? IsTransient(object entity) =>
            entity is ITrackableEntity trackableEntity
            ? trackableEntity.IsTransient
            : null;

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            SetAsLoadedOrSaved(entity);

            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "Overriden")]
        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            SetAsLoadedOrSaved(entity);

            return false;
        }

        private static void SetAsLoadedOrSaved(object entity)
        {
            if (entity is ITrackableEntity trackableEntity)
            {
                trackableEntity.SetAsLoadedOrSaved();
            }
        }
    }
}
