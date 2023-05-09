using System.Security.Claims;
using Fanzoo.Kernel.Services;

namespace Fanzoo.Kernel.Data.Listeners
{

    public sealed class AuditListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            InsertAuditFields(@event);

            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(OnPreInsert(@event));
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            UpdateAuditFields(@event);

            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(OnPreUpdate(@event));
        }

        private static void InsertAuditFields(PreInsertEvent @event)
        {
            if (@event.Entity is IImmutableEntity)
            {
                var user = @event.GetService<IContextAccessorService>()?.User?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";

                if (@event.Persister.PropertyNames.Contains("CreatedDate"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedDate")] = SystemDateTime.UtcNow;
                }

                if (@event.Persister.PropertyNames.Contains("CreatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedBy")] = user;
                }
            }
        }

        private static void UpdateAuditFields(PreUpdateEvent @event)
        {
            if (@event.Entity is IMutableEntity)
            {
                var user = @event.GetService<IContextAccessorService>()?.User?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";

                if (@event.Persister.PropertyNames.Contains("CreatedDate"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedDate")] = @event.OldState[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedDate")];
                }

                if (@event.Persister.PropertyNames.Contains("CreatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedBy")] = @event.OldState[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedBy")];
                }

                if (@event.Persister.PropertyNames.Contains("UpdatedDate"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedDate")] = SystemDateTime.UtcNow;
                }

                if (@event.Persister.PropertyNames.Contains("UpdatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedBy")] = user;
                }
            }
        }
    }
}
