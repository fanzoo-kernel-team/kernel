using Dapper;
using Fanzoo.Kernel.Configuration;
using Fanzoo.Kernel.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Fanzoo.Kernel.Queries
{
    public abstract class QueryHandler<TQuery, ResultType> : IQueryHandler<TQuery, ResultType> where TQuery : IQuery
    {
        private readonly ILogger _logger;
        private readonly IDynamicMappingService _mapper;
        protected readonly IConfiguration _configuration;
        protected readonly IEmbeddedResourceReaderService? _embeddedResourceReaderService;
        private readonly IScriptEmbeddedResourceLocator? _embeddedResourceLocator;

        protected QueryHandler(IConfiguration configuration, IDynamicMappingService mapper, IEmbeddedResourceReaderService embeddedResourceReaderService, IScriptEmbeddedResourceLocator embeddedResourceLocator)
        {
            _configuration = configuration;
            _embeddedResourceReaderService = embeddedResourceReaderService;
            _embeddedResourceLocator = embeddedResourceLocator;

            _logger = Log.ForContext<TQuery>();

            _mapper = mapper;
        }

        protected QueryHandler(IConfiguration configuration, IDynamicMappingService mapper)
        {
            _configuration = configuration;
            _logger = Log.ForContext<TQuery>();
            _mapper = mapper;
        }

        public async Task<QueryResult<ResultType>> HandleAsync(TQuery query)
        {
            try
            {
                return await OnHandleAsync(query);
            }
            catch (Exception e)
            {
                _logger.Error(e, "An error occurred processing a QueryHandler");

                return QueryResult<ResultType>.Fail(e);
            }
        }

        protected SqlConnection GetConnection() => new(_configuration.GetConnectionString());

        protected async Task<QueryResult<IEnumerable<BaseType>>> QueryFromScriptAsync<BaseType>(string script, object? parameters = null)
        {
            if (_embeddedResourceLocator is null)
            {
                throw new InvalidOperationException(nameof(_embeddedResourceLocator) + "not initialized.");
            }

            if (_embeddedResourceReaderService is null)
            {
                throw new InvalidOperationException(nameof(_embeddedResourceReaderService) + "not initialized.");
            }

            using var connection = GetConnection();

            var sql = await _embeddedResourceReaderService.ReadEmbeddedResourceFileAsync(script, _embeddedResourceLocator.Assembly);

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<IEnumerable<BaseType>>.Success(_mapper.Map<BaseType>(results));
        }

        protected async Task<QueryResult<IEnumerable<BaseType>>> QueryFromSqlAsync<BaseType>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<IEnumerable<BaseType>>.Success(_mapper.Map<BaseType>(results));
        }

        protected async Task<QueryResult<BaseType>> QuerySingleFromScriptAsync<BaseType>(string script, object? parameters = null)
        {
            if (_embeddedResourceLocator is null)
            {
                throw new InvalidOperationException(nameof(_embeddedResourceLocator) + "not initialized.");
            }

            if (_embeddedResourceReaderService is null)
            {
                throw new InvalidOperationException(nameof(_embeddedResourceReaderService) + "not initialized.");
            }

            using var connection = GetConnection();

            var sql = await _embeddedResourceReaderService.ReadEmbeddedResourceFileAsync(script, _embeddedResourceLocator.Assembly);

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<BaseType>.Success(_mapper.Map<BaseType>(results).Single());
        }

        protected async Task<QueryResult<BaseType>> QuerySingleFromSqlAsync<BaseType>(string sql, object? parameters = null)
        {
            using var connection = GetConnection();

            var results = await connection.QueryAsync<dynamic>(sql, parameters);

            return QueryResult<BaseType>.Success(_mapper.Map<BaseType>(results).Single());
        }

        protected async Task<string> GetSqlAsync(string script) => _embeddedResourceReaderService is null || _embeddedResourceLocator is null
                ? throw new InvalidOperationException(nameof(_embeddedResourceReaderService) + " or " + nameof(_embeddedResourceLocator) + "not initialized.")
                : await _embeddedResourceReaderService.ReadEmbeddedResourceFileAsync(script, _embeddedResourceLocator.Assembly);

        protected abstract Task<QueryResult<ResultType>> OnHandleAsync(TQuery query);

    }
}
