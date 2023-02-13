namespace Fanzoo.Kernel.Data.Listeners
{

    public sealed class AuditListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            if (@event.Entity is IImmutableEntity)
            {
                var user = @event.GetService<IContextAccessorService>()?.GetUser()?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";

                if (@event.Persister.PropertyNames.Contains("CreatedDate"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedDate")] = SystemDateTime.Now;
                }

                if (@event.Persister.PropertyNames.Contains("CreatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedBy")] = user;
                }
            }

            return false;
        }

        public async Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (@event.Entity is IImmutableEntity)
            {
                var user = "system";

                var context = @event.GetService<IContextAccessorService>();

                if (context is not null)
                {
                    user = (await context.GetUserAsync())?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";
                }

                if (@event.Persister.PropertyNames.Contains("CreatedDate"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedDate")] = SystemDateTime.Now;
                }

                if (@event.Persister.PropertyNames.Contains("CreatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "CreatedBy")] = user;
                }
            }

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            if (@event.Entity is IMutableEntity)
            {
                var user = @event.GetService<IContextAccessorService>()?.GetUser()?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";

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
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedDate")] = SystemDateTime.Now;
                }

                if (@event.Persister.PropertyNames.Contains("UpdatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedBy")] = user;
                }
            }

            return false;
        }

        public async Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (@event.Entity is IMutableEntity)
            {
                var user = "system";

                var context = @event.GetService<IContextAccessorService>();

                if (context is not null)
                {
                    user = (await context.GetUserAsync())?.GetClaimOrDefault(Web.ClaimTypes.Username)?.Value ?? "system";
                }

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
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedDate")] = SystemDateTime.Now;
                }

                if (@event.Persister.PropertyNames.Contains("UpdatedBy"))
                {
                    @event.State[Array.IndexOf<string>(@event.Persister.PropertyNames, "UpdatedBy")] = user;
                }
            }

            return false;
        }
    }
}
