namespace Fanzoo.Kernel.Queries
{
    public static class Extensions
    {
        public static async ValueTask<TResult> DispatchQueryAsync<TResult>(this QueryDispatcher queryDispatcher, IQuery query)
        {
            var queryResult = await queryDispatcher.DispatchAsync<TResult>(query);

            return queryResult.IsSuccessful ? queryResult.Value : throw new InvalidOperationException(queryResult.Error);
        }

        public static async ValueTask<TMappedResult> DispatchQueryAndMapAsync<TQueryResult, TMappedResult>(this QueryDispatcher queryDispatcher, IQuery query, Func<TQueryResult, TMappedResult> map)
        {
            var queryResult = await queryDispatcher.DispatchAsync<TQueryResult>(query);

            return queryResult.IsSuccessful ? map(queryResult.Value) : throw new InvalidOperationException(queryResult.Error);
        }

        public static async ValueTask<IEnumerable<TMappedResult>> DispatchQueryAndMapManyAsync<TQueryResult, TMappedResult>(this QueryDispatcher queryDispatcher, IQuery query, Func<IEnumerable<TQueryResult>, IEnumerable<TMappedResult>> map)
        {
            var queryResult = await queryDispatcher.DispatchAsync<IEnumerable<TQueryResult>>(query);

            return queryResult.IsSuccessful ? map(queryResult.Value) : throw new InvalidOperationException(queryResult.Error);
        }
    }
}
