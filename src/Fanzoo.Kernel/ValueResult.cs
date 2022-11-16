namespace Fanzoo.Kernel
{
    public sealed class ValueResult<TValue, TError> where TError : Error
    {
        private readonly TValue? _value;
        private readonly TError? _error;

        private ValueResult(bool isSuccessful, TValue? value = default, TError? error = default)
        {
            IsSuccessful = isSuccessful;
            _value = value;
            _error = error;
        }

        public static ValueResult<TValue, TError> Success(TValue value) => new(true, value);

        public static ValueResult<TValue, TError> Fail(TError error) => new(false, error: error);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2372:Exceptions should not be thrown from property getters", Justification = "Intended behavior")]
        public TValue Value
        {
            get
            {
                if (IsSuccessful && _value is not null)
                {
                    return _value;
                }

                if (IsFailure && _error is not null)
                {
                    throw new KernelErrorException(_error);
                }

                throw new InvalidOperationException();
            }
        }

        public TError Error => IsFailure && _error is not null ? _error : throw new InvalidOperationException();

        public bool IsSuccessful { get; init; }

        public bool IsFailure => !IsSuccessful;

        public static implicit operator ValueResult<TValue, TError>(TValue value) => new(true, value);

        public static implicit operator ValueResult<TValue, TError>(TError error) => new(false, error: error);
    }
}
