namespace Fanzoo.Kernel.Domain.Values
{
    //TODO: refactor this to be configurable somehow
    //service?
    public sealed class PasswordValue : RequiredStringValue, IPasswordValue
    {
        private PasswordValue() { } //ORM

        public PasswordValue(string password) : base(password)
        {
            Guard.Against.InvalidPassword(password, nameof(password));
        }

        public static ValueResult<PasswordValue, Error> Create(string password) => CanCreate(password) ? new PasswordValue(password) : Errors.ValueObjects.PasswordValue.InvalidFormat;

        public static implicit operator PasswordValue(string s) => new(s);

        public static bool CanCreate(string password) => Check.For.IsValidPassword(password);
    }
}
