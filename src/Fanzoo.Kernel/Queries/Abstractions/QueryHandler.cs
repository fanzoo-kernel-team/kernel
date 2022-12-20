using Dapper;
using Fanzoo.Kernel.Logging;
using Microsoft.Data.SqlClient;
using Serilog;

namespace Fanzoo.Kernel.Queries
{
    public interface IQueryHandler<IQuery, ResultType>
    {
        Task<QueryResult<ResultType>> HandleAsync(IQuery query);
    }

    public abstract class QueryHandler<TQuery, ResultType> : IQueryHandler<TQuery, ResultType> where TQuery : IQuery
    {
        private readonly IDynamicMappingService _mapper;
        protected readonly IConfiguration _configuration;
        protected readonly IEmbeddedResourceReaderService? _embeddedResourceReaderService;
        private readonly IScriptEmbeddedResourceLocator? _embeddedResourceLocator;

        protected QueryHandler(IConfiguration configuration, IDynamicMappingService mapper, IEmbeddedResourceReaderService embeddedResourceReaderService, IScriptEmbeddedResourceLocator embeddedResourceLocator)
        {
            _configuration = configuration;
            _embeddedResourceReaderService = embeddedResourceReaderService;
            _embeddedResourceLocator = embeddedResourceLocator;

            _mapper = mapper;
        }

        protected QueryHandler(IConfiguration configuration, IDynamicMappingService mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        protected ILogger Logger => Log.Logger;

        public async Task<QueryResult<ResultType>> HandleAsync(TQuery query)
        {
            Logger.QueryInformation<TQuery>("Begin ---------->");

            try
            {
                return await OnHandleAsync(query);
            }
            catch (Exception e)
            {
                Logger.QueryException<TQuery>(e);

                return QueryResult<ResultType>.Fail(e);
            }
            finally
            {
                Logger.QueryInformation<TQuery>("<---------- End");
            }
        }

        protected SqlConnection GetConnection() => new(_configuration.GetConnectionString());

        protected async Task<QueryResult<IEnumerable<BaseType>>> QueryFromScriptAsync<BaseType>(string script, object? parameters = null)
        {
            using var connection = GetConnection();

            var sql = await GetSqlAsync(script);

            Logger.QueryInformation<TQuery>($"Executing SQL:\r\n{sql}");

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<IEnumerable<BaseType>>.Success(_mapper.Map<BaseType>(results));
        }

        protected async Task<QueryResult<IEnumerable<BaseType>>> QueryFromSqlAsync<BaseType>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();

            Logger.QueryInformation<TQuery>($"Executing SQL:\r\n{sql}");

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<IEnumerable<BaseType>>.Success(_mapper.Map<BaseType>(results));
        }

        protected async Task<QueryResult<BaseType>> QuerySingleFromScriptAsync<BaseType>(string script, object? parameters = null)
        {
            using var connection = GetConnection();

            var sql = await GetSqlAsync(script);

            Logger.QueryInformation<TQuery>($"Executing SQL:\r\n{sql}");

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<BaseType>.Success(_mapper.Map<BaseType>(results).Single());
        }

        protected async Task<QueryResult<BaseType>> QuerySingleFromSqlAsync<BaseType>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();

            Logger.QueryInformation<TQuery>($"Executing SQL:\r\n{sql}");

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<BaseType>.Success(_mapper.Map<BaseType>(results).Single());
        }

        protected async Task<string> GetSqlAsync(string script) => _embeddedResourceReaderService is null || _embeddedResourceLocator is null
                ? throw new InvalidOperationException(nameof(_embeddedResourceReaderService) + " or " + nameof(_embeddedResourceLocator) + "not initialized.")
                : await _embeddedResourceReaderService.ReadEmbeddedResourceFileAsync(script, _embeddedResourceLocator.Assembly);

        protected abstract Task<QueryResult<ResultType>> OnHandleAsync(TQuery query);

    }
}
