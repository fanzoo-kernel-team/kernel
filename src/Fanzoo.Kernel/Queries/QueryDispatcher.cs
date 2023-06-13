namespace Fanzoo.Kernel.Queries
{
    public sealed class QueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<QueryResult<T>> DispatchAsync<T>(IQuery query)
        {
            dynamic QueryHandler = _serviceProvider
                .GetService(typeof(IQueryHandler<,>)
                    .MakeGenericType(query.GetType(), typeof(T)))!;

            return await QueryHandler.HandleAsync((dynamic)query);
        }
    }
}
