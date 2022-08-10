namespace Fanzoo.Kernel.Queries
{
    public class QueryResult<T>
    {
        public bool IsSuccessful { get; }

        public string? Error { get; }

        public T Value { get; } = default!;

        protected QueryResult(bool isSuccessful, string? error)
        {
            IsSuccessful = isSuccessful;
            Error = error;
        }

        protected QueryResult(bool isSuccessful, string? error, T value) : this(isSuccessful, error)
        {
            Value = value;
        }

        public static QueryResult<T> Success(T value) => new(true, null, value);

        public static QueryResult<T> Fail() => new(false, null);

        public static QueryResult<T> Fail(string error) => new(false, error);

        public static QueryResult<T> Fail(Exception exception) => new(false, exception.Message);

    }
}
