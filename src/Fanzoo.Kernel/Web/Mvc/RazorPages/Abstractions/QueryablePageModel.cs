using Microsoft.AspNetCore.Mvc.RazorPages;
using KernelQueries = Fanzoo.Kernel.Queries;

namespace Fanzoo.Kernel.Web.Mvc.RazorPages
{
    public abstract class QueryablePageModel(KernelQueries.QueryDispatcher queryDispatcher) : PageModel
    {
        protected KernelQueries.QueryDispatcher QueryDispatcher { get; } = queryDispatcher;

        protected async ValueTask<TResult> DispatchQueryAsync<TResult>(KernelQueries.IQuery query)
        {
            var result = await QueryDispatcher.DispatchAsync<TResult>(query);

            return result.IsSuccessful is false ? throw new InvalidOperationException(result.Error) : result.Value;
        }

        protected async ValueTask<TMappedResult> DispatchQueryAndMapAsync<TQueryResult, TMappedResult>(KernelQueries.IQuery query, Func<TQueryResult, TMappedResult> map)
        {
            var result = await QueryDispatcher.DispatchAsync<TQueryResult>(query);

            return result.IsSuccessful is false ? throw new InvalidOperationException(result.Error) : map(result.Value);
        }

        protected async ValueTask<IEnumerable<TMappedResult>> DispatchQueryAndMapManyAsync<TQueryResult, TMappedResult>(KernelQueries.IQuery query, Func<IEnumerable<TQueryResult>, IEnumerable<TMappedResult>> map)
        {
            var result = await QueryDispatcher.DispatchAsync<IEnumerable<TQueryResult>>(query);

            return result.IsSuccessful is false ? throw new InvalidOperationException(result.Error) : map(result.Value);
        }
    }
}
