using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Data.Repositories;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands
{
    public record CreateCommand(string Name) : ICommand;

    public class CreateCommandHandler : CommandHandler<CreateCommand>
    {
        private readonly IGameRepository _gameRepository;

        public CreateCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher, IGameRepository gameRepository) : base(unitOfWorkFactory, eventDispatcher)
        {
            _gameRepository = gameRepository;
        }

        protected override async Task<CommandResult> OnHandleAsync(CreateCommand command)
        {
            var game = Game.Create(command.Name);

            await _gameRepository.AddAsync(game);

            return CommandResult.Success();
        }
    }
}