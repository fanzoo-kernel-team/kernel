namespace Fanzoo.Kernel
{
    public class Error
    {
        public static Error UnspecifiedError => new("unspecified.error", "Unspecified Error");

        public Error()
        {
            Code = string.Empty;
            Message = string.Empty;
        }

        public Error(string code, string message)
        {
            Guard.Against.NullOrWhiteSpace(code, nameof(code));

            Guard.Against.NullOrWhiteSpace(message, nameof(message));

            Code = code;
            Message = message;
        }

        public string Code { get; init; }

        public string Message { get; init; }

        public override string ToString() => Message;
    }
}
