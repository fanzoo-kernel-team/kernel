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

        public static ValueResult<EmailValue, Error> Create(string email)
        {
            email = email.ToLower().Trim();

            var isValid = Check.For
                .NotNullOrWhiteSpace(email)
                .And
                .LessThanOrEqual(email.Length, MAX_SIZE)
                .And
                .IsValidEmailFormat(email);


            return isValid ? new EmailValue(email) : Errors.ValueObjects.EmailValue.InvalidFormat;

        }

        public static implicit operator EmailValue(string value) => new(value);

    }
}
