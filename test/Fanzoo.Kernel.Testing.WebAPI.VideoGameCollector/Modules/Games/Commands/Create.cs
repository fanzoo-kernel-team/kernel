namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands
{
    public record CreateCommand(string Name) : ICommand;

    public class CreateCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher, IGameRepository gameRepository) : CommandHandler<CreateCommand>(unitOfWorkFactory, eventDispatcher)
    {
        private readonly IGameRepository _gameRepository = gameRepository;

        protected override async Task<CommandResult> OnHandleAsync(CreateCommand command)
        {
            var game = Game.Create(command.Name);

            await _gameRepository.AddAsync(game);

            return CommandResult.Success();
        }
    }
}