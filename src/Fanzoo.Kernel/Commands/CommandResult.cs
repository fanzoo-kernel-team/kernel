namespace Fanzoo.Kernel.Commands
{
    public sealed class CommandResult
    {
        public bool IsSuccessful { get; }

        public string? ErrorMessage { get; }

        public Error? Error { get; }

        private CommandResult(bool isSuccessful, Error? error = null, string? errorMessage = null)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            ErrorMessage = errorMessage;
        }

        public static CommandResult Success() => new(true);

        public static CommandResult Fail() => new(false);

        public static CommandResult Fail(string error) => new(false, null, error);

        public static CommandResult Fail(Exception exception) => new(false, null, exception.Message);

        public static CommandResult Fail(Error error) => new(false, error, error.Message);

    }

    public sealed class CommandResult<T>
    {
        public bool IsSuccessful { get; }

        public string? ErrorMessage { get; }

        public Error? Error { get; }

        public T Value { get; } = default!;

        private CommandResult(bool isSuccessful, Error? error = null, string? errorMessage = null)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            ErrorMessage = errorMessage;
        }

        private CommandResult(T value) : this(true)
        {
            Value = value;
        }

        public static CommandResult<T> Success(T value) => new(value);

        public static CommandResult<T> Fail() => new(false);

        public static CommandResult<T> Fail(string error) => new(false, null, error);

        public static CommandResult<T> Fail(Exception exception) => new(false, null, exception.Message);

        public static CommandResult<T> Fail(Error error) => new(false, error, error.Message);

    }
}
