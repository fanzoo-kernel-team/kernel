namespace Fanzoo.Kernel
{
    public class Check
    {
        private enum Operation
        {
            And,
            Or
        }

        private readonly Operation _operation;
        private bool _resolved;
        private bool _result;

        private Check(bool result, Operation operation, bool resolved = false)
        {
            _result = result;
            _operation = operation;
            _resolved = resolved;
        }

        public static Check For => new(true, Operation.And);

        public Check Resolve(bool result)
        {
            if (_resolved)
            {
                throw new InvalidOperationException("Check is already resolved.");
            }

            _resolved = true;

            switch (_operation)
            {
                case Operation.And:
                    _result &= result;
                    return this;

                case Operation.Or:
                    _result |= result;
                    return this;

                default:
                    throw new InvalidOperationException("Not a valid operation.");
            }
        }

        public Check And => new(Result, Operation.And);

        public Check Or => new(Result, Operation.Or);

        public bool Result => !_resolved ? throw new InvalidOperationException("Check is not resolved.") : _result;

        public static implicit operator bool(Check check) => check.Result;

        public static implicit operator Check(bool result) => new(result, Operation.And, true);
    }
}
