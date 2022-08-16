namespace Fanzoo.Kernel.Queries
{
    public interface IQueryHandler<IQuery, ResultType>
    {
        Task<QueryResult<ResultType>> HandleAsync(IQuery query);
    }
}
