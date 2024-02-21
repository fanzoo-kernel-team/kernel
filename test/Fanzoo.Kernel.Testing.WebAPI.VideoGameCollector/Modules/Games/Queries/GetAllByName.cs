namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries
{
    public record GetAllByNameQuery(string Name) : IQuery;

    public record struct GameDetailResult(string Name);

    public sealed class GetAllByNameQueryHandler(IConfiguration configuration, IDynamicMappingService mapper) : QueryHandler<GetAllByNameQuery, IEnumerable<GameDetailResult>>(configuration, mapper)
    {
        protected override async Task<QueryResult<IEnumerable<GameDetailResult>>> OnHandleAsync(GetAllByNameQuery query) => await QueryFromSqlAsync<GameDetailResult>("SELECT [Name] FROM [dbo].[Game] WHERE [Name] = @Name", new { query.Name });
    }
}
