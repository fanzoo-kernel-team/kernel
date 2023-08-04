namespace Fanzoo.Kernel.Queries
{
    public abstract class DispatchAndQueryResultCore
    {
        private readonly string? _error;

        protected DispatchAndQueryResultCore(bool isSuccessful, string? error)
        {
            IsSuccessful = isSuccessful;
            _error = error;
        }

        public string Error => IsSuccessful is false && _error is not null ? _error : throw new InvalidOperationException($"Cannot access value of property {nameof(Error)} when result is sucessful.");

        public bool IsSuccessful { get; }
    }

    public sealed class QueryDispatchAndMapResult<TMappedResult> : DispatchAndQueryResultCore
    {
        private readonly TMappedResult? _value;

        private QueryDispatchAndMapResult(bool isSuccessful, TMappedResult? value, string? error = null) : base(isSuccessful, error)
        {
            _value = value;
        }

        public static QueryDispatchAndMapResult<TMappedResult> Success(TMappedResult value) => new(true, value);

        public static QueryDispatchAndMapResult<TMappedResult> Fail(string error) => new(false, default, error);

        public TMappedResult Value => IsSuccessful && _value is not null ? _value : throw new InvalidOperationException($"Cannot access value of property {nameof(Value)} when result is unsucessful.");
    }

    public sealed class QueryDispatchResultAndMapManyResult<TMappedResult> : DispatchAndQueryResultCore
    {
        private readonly IEnumerable<TMappedResult>? _value;

        private QueryDispatchResultAndMapManyResult(bool isSuccessful, IEnumerable<TMappedResult>? value, string? error = null) : base(isSuccessful, error)
        {
            _value = value;
        }

        public static QueryDispatchResultAndMapManyResult<TMappedResult> Success(IEnumerable<TMappedResult> value) => new(true, value);

        public static QueryDispatchResultAndMapManyResult<TMappedResult> Fail(string error) => new(false, default, error);

        public IEnumerable<TMappedResult> Value => IsSuccessful && _value is not null ? _value : throw new InvalidOperationException($"Cannot access value of property {nameof(Value)} when result is unsucessful.");
    }

    public static class Extensions
    {
        public static async ValueTask<QueryDispatchAndMapResult<TResult>> DispatchQueryAsync<TResult>(this QueryDispatcher queryDispatcher, IQuery query)
        {
            var queryResult = await queryDispatcher.DispatchAsync<TResult>(query);

            return queryResult.IsSuccessful
                ? QueryDispatchAndMapResult<TResult>.Success(queryResult.Value)
                : QueryDispatchAndMapResult<TResult>.Fail(queryResult.Error!);
        }

        public static async ValueTask<QueryDispatchAndMapResult<TMappedResult>> DispatchQueryAndMapAsync<TQueryResult, TMappedResult>(this QueryDispatcher queryDispatcher, IQuery query, Func<TQueryResult, TMappedResult> map)
        {
            var queryResult = await queryDispatcher.DispatchAsync<TQueryResult>(query);

            return queryResult.IsSuccessful
                ? QueryDispatchAndMapResult<TMappedResult>.Success(map(queryResult.Value))
                : QueryDispatchAndMapResult<TMappedResult>.Fail(queryResult.Error!);
        }

        public static async ValueTask<QueryDispatchResultAndMapManyResult<TMappedResult>> DispatchQueryAndMapManyAsync<TQueryResult, TMappedResult>(this QueryDispatcher queryDispatcher, IQuery query, Func<IEnumerable<TQueryResult>, IEnumerable<TMappedResult>> map)
        {
            var queryResult = await queryDispatcher.DispatchAsync<IEnumerable<TQueryResult>>(query);

            return queryResult.IsSuccessful
                ? QueryDispatchResultAndMapManyResult<TMappedResult>.Success(map(queryResult.Value))
                : QueryDispatchResultAndMapManyResult<TMappedResult>.Fail(queryResult.Error!);
        }
    }
}
