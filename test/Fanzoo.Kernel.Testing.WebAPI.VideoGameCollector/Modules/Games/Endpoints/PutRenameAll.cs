using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Commands;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Endpoints
{
    public record struct RenameAllRequest(string OldName, string NewName);

    public static class PutRenameAll
    {
        public static async Task<IResult> HandleAsync(RenameAllRequest request, CommandDispatcher commandDispatcher)
        {
            var result = await commandDispatcher.DispatchAsync(new RenameAllCommand(request.OldName, request.NewName));

            return result.IsSuccessful ? Results.Ok() : Results.BadRequest();
        }
    }
}
