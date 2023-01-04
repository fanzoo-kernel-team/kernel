namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class UrlValue : RequiredStringValue
    {
        public const int MAX_SIZE = 2048;

        private UrlValue() { }

        public UrlValue(string Url) : base(Url)
        {
            Guard.Against.NullOrWhiteSpace(Url, nameof(Url));
            Guard.Against.ExceedsMaxValue(Url.Length, MAX_SIZE, nameof(Url));
            Guard.Against.InvalidUrlFormat(Url, nameof(Url));
        }

        public static ValueResult<UrlValue, Error> Create(string Url)
        {
            Url = Url.ToLower().Trim();

            var isValid = Check.For
                .NullOrWhiteSpace(Url)
                .And
                .ExceedsMaxValue(Url.Length, MAX_SIZE)
                .And
                .ValidUrlFormat(Url)
                    .IsValid;


            return isValid ? new UrlValue(Url) : Errors.ValueObjects.UrlValue.InvalidFormat;

        }

        public static implicit operator UrlValue(string value) => new(value);

    }
}
