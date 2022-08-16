using Fanzoo.Kernel.Data;
using NHibernate.Engine;
using ISession = NHibernate.ISession;

namespace Fanzoo.Kernel.Commands;

internal static class IScopedUnitOfWorkExtensions
{
    public static IEnumerable<IAggregateRoot> GetEntitiesWithEvents(this IScopedUnitOfWork unitOfWork)
    {
        if (unitOfWork.GetFieldValue("_session") is not ISession session)
        {
            yield break;
        }

        var sessionImplementation = session.GetSessionImplementation();

        foreach (EntityEntry entry in sessionImplementation.PersistenceContext.EntityEntries.Values)
        {
            var entity = sessionImplementation.PersistenceContext.GetEntity(entry.EntityKey);

            if (entity is IAggregateRoot aggregateRoot && aggregateRoot.Events.Count > 0)
            {
                yield return aggregateRoot;
            }
        }

        yield break;
    }
}

