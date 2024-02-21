
namespace System
{
    public class KernelErrorException(Error error) : Exception(error.Message)
    {
        private readonly Error _error = error;

        public string Code => _error.Code;
    }
}
