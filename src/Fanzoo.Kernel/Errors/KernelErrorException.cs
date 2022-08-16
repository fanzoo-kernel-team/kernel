#pragma warning disable S3925 // "ISerializable" should be implemented correctly

namespace System
{
    public class KernelErrorException : Exception
    {
        private readonly Error _error;

        public KernelErrorException(Error error) : base(error.Message)
        {
            _error = error;
        }

        public string Code => _error.Code;
    }
}
