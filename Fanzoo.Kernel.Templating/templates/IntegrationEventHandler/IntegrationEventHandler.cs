using {project}.Events.{area};

namespace {project}.EventHandlers.{area}
{
    public class {name}EventHandler : IntegrationEventHandler<{name}Event>
    {
        public {name}EventHandler(IReadOnlyUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected override async ValueTask OnHandleAsync({name}Event @event)
        {
            //event logic here
        }
    }
}