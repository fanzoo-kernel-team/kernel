namespace Fanzoo.Kernel.Domain.Values
{
    public partial class CssColorValue : ValueObject
    {
        public CssColorValue(string value)
        {
            if (!CanCreate(value))
            {
                throw new ArgumentOutOfRangeException($"{value} is not a valid CSS color.");
            }

            Value = value;
        }

        public static ValueResult<CssColorValue, Error> Create(string value) =>
            CanCreate(value)
                ? new CssColorValue(value)
                : Errors.ValueObjects.CssColorValue.InvalidFormat;

        public string Value { get; private set; }

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return Value;
        }

        public static bool CanCreate(string value) => Check.For.IsValidCssColor(value);
    }
}
