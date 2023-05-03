namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class FilenameValue : RequiredStringValue
    {
        private FilenameValue() { }

        public FilenameValue(string filename) : base(filename)
        {
            Guard.Against.NullOrWhiteSpace(filename, nameof(filename));
            Guard.Against.ExceedsMaxValue(filename.Length, DatabaseCatalog.FieldLength.FileName, nameof(filename));
            Guard.Against.InvalidFilename(filename, nameof(filename));
        }
        public static ValueResult<FilenameValue, Error> Create(string filename) => CanCreate(filename) ? new FilenameValue(filename) : Errors.ValueObjects.FilenameValue.InvalidFormat;

        public static implicit operator FilenameValue(string value) => new(value);

        public static bool CanCreate(string filename)
        {
            filename = filename.ToLower().Trim();
            return Check.For
                .NotNullOrWhiteSpace(filename)
                .And
                .LessThanOrEqual(filename.Length, DatabaseCatalog.FieldLength.FileName)
                .And
                .IsValidFilename(filename);
        }
    }
}
