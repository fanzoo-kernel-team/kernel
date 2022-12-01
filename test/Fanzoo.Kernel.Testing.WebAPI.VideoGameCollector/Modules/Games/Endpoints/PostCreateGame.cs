using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints
{
    public static class PostCreate
    {
        public static async Task<Results<Ok, BadRequest>> HandleAsync(string name, CommandDispatcher commandDispatcher)
        {
            var result = await commandDispatcher.DispatchAsync(new CreateCommand(name));

            return result.IsSuccessful ? TypedResults.Ok() : TypedResults.BadRequest();
        }
    }
}
