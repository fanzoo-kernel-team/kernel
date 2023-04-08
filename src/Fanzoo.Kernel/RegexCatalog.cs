namespace Fanzoo.Kernel
{
    public static partial class RegexCatalog
    {
        [GeneratedRegex(@"^#(?:[0-9a-fA-F]{3}){1,2}$")]
        public static partial Regex CssColor();

        [GeneratedRegex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public static partial Regex Phone();

        [GeneratedRegex(@"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$")]
        public static partial Regex IPAddressPattern();

        [GeneratedRegex(@"^\d{5}(?:[-\s]\d{4})?$")]
        public static partial Regex PostalCode();
    }
}
