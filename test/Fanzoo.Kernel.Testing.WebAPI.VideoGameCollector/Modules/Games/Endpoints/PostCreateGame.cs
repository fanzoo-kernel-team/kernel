using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints
{
    public static class PostCreate
    {
        public static async Task<IResult> HandleAsync(string name, CommandDispatcher commandDispatcher)
        {
            var result = await commandDispatcher.DispatchAsync(new CreateCommand(name));

            return result.IsSuccessful ? Results.Ok() : Results.BadRequest();
        }
    }
}
