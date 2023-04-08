namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class EmailUsernameValue : RequiredStringValue, IUsernameValue
    {
        public const int MAX_SIZE = 254;

        private EmailUsernameValue() { } //ORM

        public EmailUsernameValue(string username) : base(username)
        {
            username = username.ToLower().Trim();

            Guard.Against.NullOrWhiteSpace(username, nameof(username));
            Guard.Against.ExceedsMaxValue(username.Length, MAX_SIZE, nameof(username));
            Guard.Against.InvalidEmailFormat(username, nameof(username));
        }

        public static ValueResult<EmailUsernameValue, Error> Create(string username) => CanCreate(GetUsername(username))              
                ? new EmailUsernameValue(GetUsername(username)) 
                : Errors.ValueObjects.UsernameValue.InvalidFormat;
        

        public static implicit operator EmailUsernameValue(string s) => new(s);

        public static bool CanCreate(string username) => Check.For.IsValidEmailFormat(GetUsername(username));
        private static string GetUsername(string username) => username
                 .ToLower()
                     .Trim();
    }
}
