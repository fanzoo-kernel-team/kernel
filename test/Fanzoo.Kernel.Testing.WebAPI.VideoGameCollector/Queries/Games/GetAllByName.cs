using Fanzoo.Kernel.Queries;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Dtos;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Queries.Games
{
    public record GetAllByNameQuery(string Name) : IQuery;

    public sealed class GetAllByNameQueryHandler : QueryHandler<GetAllByNameQuery, IEnumerable<GameDetailsDto>>
    {
        public GetAllByNameQueryHandler(IConfiguration configuration, IDynamicMappingService mapper) : base(configuration, mapper) { }

        protected override async Task<QueryResult<IEnumerable<GameDetailsDto>>> OnHandleAsync(GetAllByNameQuery query)
        {
            return await QueryFromSqlAsync<GameDetailsDto>("SELECT [Name] FROM [dbo].[Game] WHERE [Name] = @Name", new { query.Name });
        }
    }
}
