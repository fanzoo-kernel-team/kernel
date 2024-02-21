namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands
{
    public record RenameAllCommand(string OldName, string NewName) : ICommand;

    public sealed class RenameAllCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher, IGameRepository gameRepository) : CommandHandler<RenameAllCommand>(unitOfWorkFactory, eventDispatcher)
    {
        private readonly IGameRepository _gameRepository = gameRepository;

        protected override async Task<CommandResult> OnHandleAsync(RenameAllCommand command)
        {
            foreach (var game in await _gameRepository.FindAsync(new(command.OldName)))
            {
                game.Name = new(command.NewName);
            }

            return CommandResult.Success();
        }
    }
}
