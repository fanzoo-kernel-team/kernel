namespace Fanzoo.Kernel.Commands
{
    public sealed class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<CommandResult> DispatchAsync(ICommand command)
        {
            dynamic commandHandler = _serviceProvider
                .GetService(typeof(ICommandHandler<>)
                    .MakeGenericType(new Type[] { command.GetType() }))!;

            return await commandHandler.HandleAsync((dynamic)command);
        }

        public async Task<CommandResult<T>> DispatchAsync<T>(ICommand command)
        {
            dynamic commandHandler = _serviceProvider
                .GetService(typeof(ICommandHandler<,>)
                    .MakeGenericType(new Type[] { command.GetType(), typeof(T) }))!;

            return await commandHandler.HandleAsync((dynamic)command);
        }
    }
}
