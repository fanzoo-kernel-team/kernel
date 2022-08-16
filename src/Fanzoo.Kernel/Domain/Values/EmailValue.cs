namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class EmailValue : StringValue
    {
        public const int MAX_SIZE = 254;

        private EmailValue() { }

        public EmailValue(string email) : base(email)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.ExceedsMaxValue(email.Length, MAX_SIZE, nameof(email));
            Guard.Against.InvalidEmailFormat(email, nameof(email));
        }

        public static Result<EmailValue, Error> Create(string email)
        {
            email = email.ToLower().Trim();

            var isValid = Check.For
                .NullOrWhiteSpace(email)
                .And
                .ExceedsMaxValue(email.Length, MAX_SIZE)
                .And
                .ValidEmailFormat(email)
                    .IsValid;


            return isValid ? new EmailValue(email) : Errors.ValueObjects.EmailValue.InvalidFormat;

        }
    }
}
