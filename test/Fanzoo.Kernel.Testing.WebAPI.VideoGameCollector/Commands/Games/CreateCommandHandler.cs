using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Commands.Games;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.CommandHandlers.Games
{
    public class CreateCommandHandler : CommandHandler<CreateCommand>
    {
        public CreateCommandHandler(IScopedUnitOfWork unitOfWork, EventDispatcher eventDispatcher) : base(unitOfWork, eventDispatcher) { }

        protected override async Task<CommandResult> OnHandleAsync(CreateCommand command)
        {
            var game = Game.Create(command.Name);

            await Repository<Game>().AddAsync(game);

            return CommandResult.Success();
        }
    }
}