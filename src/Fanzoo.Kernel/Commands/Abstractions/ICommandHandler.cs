namespace Fanzoo.Kernel.Commands
{
    public interface ICommandHandler<ICommand>
    {
        Task<CommandResult> HandleAsync(ICommand command);
    }

    public interface ICommandHandler<ICommand, ResultType>
    {
        Task<CommandResult<ResultType>> HandleAsync(ICommand command);
    }
}
