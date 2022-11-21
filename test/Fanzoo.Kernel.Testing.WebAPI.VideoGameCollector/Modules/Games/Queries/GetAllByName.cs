using Fanzoo.Kernel.Queries;
using Fanzoo.Kernel.Services;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Queries
{
    public record GetAllByNameQuery(string Name) : IQuery;

    public record struct GameDetailResult(string Name);

    public sealed class GetAllByNameQueryHandler : QueryHandler<GetAllByNameQuery, IEnumerable<GameDetailResult>>
    {
        public GetAllByNameQueryHandler(IConfiguration configuration, IDynamicMappingService mapper) : base(configuration, mapper) { }

        protected override async Task<QueryResult<IEnumerable<GameDetailResult>>> OnHandleAsync(GetAllByNameQuery query) => await QueryFromSqlAsync<GameDetailResult>("SELECT [Name] FROM [dbo].[Game] WHERE [Name] = @Name", new { query.Name });
    }
}
