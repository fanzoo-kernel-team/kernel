namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class EmailValue : RequiredStringValue
    {
        public const int MAX_SIZE = 254;

        private EmailValue() { }

        public EmailValue(string email) : base(email)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.ExceedsMaxValue(email.Length, MAX_SIZE, nameof(email));
            Guard.Against.InvalidEmailFormat(email, nameof(email));
        }

        public static ValueResult<EmailValue, Error> Create(string email) =>
            CanCreate(GetEmail(email))
                ? new EmailValue(GetEmail(email))
                : Errors.ValueObjects.EmailValue.InvalidFormat;


        public static implicit operator EmailValue(string value) => new(value);

        public static bool CanCreate(string email) => Check.For.IsValidEmailFormat(GetEmail(email));

        private static string GetEmail(string email) => email
                .ToLower()
                    .Trim();
    }
}

