using {project}.Commands.{area};

namespace {project}.CommandHandlers.{area}
{
    public class {name}CommandHandler : CommandHandler<{name}Command>
    {
        public {name}CommandHandler(IScopedUnitOfWork unitOfWork, EventDispatcher eventDispatcher) : base(unitOfWork, eventDispatcher) { }

        protected override async Task<CommandResult> OnHandleAsync({name}Command command)
        {
            //load aggregate

            //do logic

            return CommandResult.Success();
        }
    }
}