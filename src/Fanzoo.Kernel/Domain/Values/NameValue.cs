namespace Fanzoo.Kernel.Domain.Values
{
    public class NameValue : ValueObject
    {
        private NameValue() { } //ORM

        public NameValue(string firstName, string lastName)
        {
            Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            FirstName = firstName;
            LastName = lastName;
        }

        public static ValueResult<NameValue, Error> Create(string firstName, string lastName)
        {
            var isValid = Check.For
                .NotNullOrWhiteSpace(firstName)
                .And
                .NotNullOrWhiteSpace(lastName);

            return isValid ? new NameValue(firstName, lastName) : Errors.ValueObjects.NameValue.InvalidFormat;
        }

        public string FirstName { get; } = default!;

        public string LastName { get; } = default!;

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
