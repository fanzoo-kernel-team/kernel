namespace Fanzoo.Kernel
{
    public static partial class RegexCatalog
    {
        [GeneratedRegex(@"^#(?:[0-9a-fA-F]{3}){1,2}$")]
        public static partial Regex CssColor();

        [GeneratedRegex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public static partial Regex Phone();

        [GeneratedRegex(@"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}(:\d+)?$")]
        public static partial Regex IPAddressPattern();

        [GeneratedRegex(@"^\d{5}(?:[-\s]\d{4})?$")]
        public static partial Regex PostalCode();

        [GeneratedRegex(@"[a-z]")]
        public static partial Regex Lowercase();

        [GeneratedRegex(@"[A-Z]")]
        public static partial Regex Uppercase();

        [GeneratedRegex(@"[0-9]")]
        public static partial Regex Digit();

        [GeneratedRegex(@"[!@#$%^&*()]")]
        public static partial Regex SpecialCharacter();

        [GeneratedRegex(@"(.)\1{2,}")]
        public static partial Regex MoreThanTwoMatchingCharactersInARow();

        [GeneratedRegex(@"\s")]
        public static partial Regex HasSpaces();

        [GeneratedRegex(@"^[^<>:""/\\|?*\x00-\x1f]+$")]
        public static partial Regex Filename();
    }
}
