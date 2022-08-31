using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Repositories;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Commands.Games
{
    public record RenameAllCommand(string OldName, string NewName) : ICommand;

    public sealed class RenameAllCommandHandler : CommandHandler<RenameAllCommand>
    {
        private readonly IGameRepository _gameRepository;

        public RenameAllCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, EventDispatcher eventDispatcher, IGameRepository gameRepository) : base(unitOfWorkFactory, eventDispatcher)
        {
            _gameRepository = gameRepository;
        }

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
