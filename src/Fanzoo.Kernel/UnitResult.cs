namespace Fanzoo.Kernel
{
    public static class UnitResult
    {
        public static UnitResult<TError> Success<TError>() where TError : Error => new(true);

        public static UnitResult<TError> Fail<TError>() where TError : Error => new(false);
    }

    public sealed class UnitResult<TError> where TError : Error
    {
        private readonly TError? _error;

        internal UnitResult(bool isSuccessful, TError? error = default)
        {
            IsSuccessful = isSuccessful;
            _error = error;
        }

        public static UnitResult<TError> Success() => new(true);

        public static UnitResult<TError> Fail(TError error) => new(false, error);

        public TError Error => IsFailure && _error is not null ? _error : throw new InvalidOperationException();

        public bool IsSuccessful { get; init; }

        public bool IsFailure => !IsSuccessful;

        public static implicit operator UnitResult<TError>(TError error) => new(false, error);

    }
}
