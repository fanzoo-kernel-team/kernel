namespace Fanzoo.Kernel
{
    public interface ICheckResult
    {
        public bool IsValid { get; }

        public bool IsInvalid { get; }

        public Check And { get; }

        public Check Or { get; }
    }

    public class Check : ICheckResult
    {
        private enum Operation
        {
            And,
            Or
        }

        private readonly Operation _operation;

        private Check(bool result, Operation operation)
        {
            Result = result;
            _operation = operation;
        }

        public static Check For => new(true, Operation.And);

        internal Check Resolve(bool result)
        {
            switch (_operation)
            {
                case Operation.And:
                    Result &= result;
                    return this;

                case Operation.Or:
                    Result |= result;
                    return this;

                default:
                    throw new InvalidOperationException("Not a valid operation.");
            }
        }

        public Check And => new(Result, Operation.And);

        public Check Or => new(Result, Operation.Or);

        private bool Result { get; set; }

        public bool IsValid => Result;

        public bool IsInvalid => !Result;
    }
}
