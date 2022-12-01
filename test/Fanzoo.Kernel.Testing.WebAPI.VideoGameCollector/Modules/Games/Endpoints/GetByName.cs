using Fanzoo.Kernel.Queries;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints
{
    public static class GetByName
    {
        public static async Task<Results<Ok<IEnumerable<GameDetailResult>>, BadRequest>> HandleAsync(string name, QueryDispatcher queryDispatcher)
        {
            var result = await queryDispatcher.DispatchAsync<IEnumerable<GameDetailResult>>(new GetAllByNameQuery(name));

            return result.IsSuccessful ? TypedResults.Ok(result.Value) : TypedResults.BadRequest();
        }
    }
}
