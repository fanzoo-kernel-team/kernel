using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Repositories;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.CommandHandlers.Games
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