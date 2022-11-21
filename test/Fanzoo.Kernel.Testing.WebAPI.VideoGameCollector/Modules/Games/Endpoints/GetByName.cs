using Fanzoo.Kernel.Queries;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints
{
    public static class GetByName
    {
        public static async Task<IResult> HandleAsync(string name, QueryDispatcher queryDispatcher)
        {
            var result = await queryDispatcher.DispatchAsync<IEnumerable<GameDetailResult>>(new GetAllByNameQuery(name));

            return result.IsSuccessful ? Results.Ok(result.Value) : Results.BadRequest();
        }
    }
}
