namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Commands.Games
{
    public record CreateCommand(string Name) : ICommand;

    public record UpdateCommand(string Name) : ICommand;
}
