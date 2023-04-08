namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class UrlValue : RequiredStringValue
    {
        public const int MAX_SIZE = 2048;

        private UrlValue() { }

        public UrlValue(string url) : base(url)
        {
            Guard.Against.NullOrWhiteSpace(url, nameof(url));
            Guard.Against.ExceedsMaxValue(url.Length, MAX_SIZE, nameof(url));
            Guard.Against.InvalidUrlFormat(url, nameof(url));
        }

        public static ValueResult<UrlValue, Error> Create(string url) => CanCreate(url) ? new UrlValue(url) : Errors.ValueObjects.UrlValue.InvalidFormat;

        public static implicit operator UrlValue(string value) => new(value);

        public static bool CanCreate(string url)
        {
            url = url.ToLower().Trim();

            return Check.For
                .NotNullOrWhiteSpace(url)
                .And
                .LessThanOrEqual(url.Length, MAX_SIZE)
                .And
                .IsValidUrlFormat(url);
        }
    }
}
