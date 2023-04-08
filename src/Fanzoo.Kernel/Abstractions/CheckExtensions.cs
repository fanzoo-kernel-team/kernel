using System.Globalization;

namespace Fanzoo.Kernel
{
    public static class CheckExtensions
    {
        //private const string PhonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        //private const string IPAddressPattern = @"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";

        //private const string PostalCodePattern = @"^\d{5}(?:[-\s]\d{4})?$";

        public static Check NotNull<T>(this Check check, T value) => check.Resolve(value is not null);

        public static Check IsNull<T>(this Check check, T value) => check.Resolve(value is null);

        public static Check NotNullOrWhiteSpace(this Check check, string value) => check.Resolve(!string.IsNullOrWhiteSpace(value));

        public static Check IsNullOrWhiteSpace(this Check check, string value) => check.Resolve(string.IsNullOrWhiteSpace(value));

        public static Check LessThan(this Check check, int value, int maximum) => check.Resolve(value < maximum);

        public static Check LessThan(this Check check, long value, long maximum) => check.Resolve(value < maximum);

        public static Check LessThan(this Check check, decimal value, decimal maximum) => check.Resolve(value < maximum);

        public static Check LessThanOrEqual(this Check check, int value, int maximum) => check.Resolve(value <= maximum);

        public static Check LessThanOrEqual(this Check check, long value, long maximum) => check.Resolve(value <= maximum);

        public static Check LessThanOrEqual(this Check check, decimal value, decimal maximum) => check.Resolve(value <= maximum);

        public static Check GreaterThan(this Check check, int value, int maximum) => check.Resolve(value > maximum);

        public static Check GreaterThan(this Check check, long value, long maximum) => check.Resolve(value > maximum);

        public static Check GreaterThan(this Check check, decimal value, decimal maximum) => check.Resolve(value > maximum);

        public static Check GreaterThanOrEqual(this Check check, int value, int minimum) => check.Resolve(value >= minimum);

        public static Check GreaterThanOrEqual(this Check check, long value, long minimum) => check.Resolve(value >= minimum);

        public static Check GreaterThanOrEqual(this Check check, decimal value, decimal minimum) => check.Resolve(value >= minimum);

        public static Check LengthIsLessThanOrEqual(this Check check, string value, int maximum) => check.Resolve(value.Length <= maximum);

        public static Check LengthIsLessThan(this Check check, string value, int maximum) => check.Resolve(value.Length < maximum);

        public static Check LengthIsGreaterThanOrEqual(this Check check, string value, int minimum) => check.Resolve(value.Length >= minimum);

        public static Check LengthIsGreaterThan(this Check check, string value, int minimum) => check.Resolve(value.Length > minimum);

        public static Check Matches(this Check check, string value, string pattern) => check.Resolve(Regex.IsMatch(value, pattern));

        public static Check DoesNotMatch(this Check check, string value, string pattern) => check.Resolve(!Regex.IsMatch(value, pattern));

        public static Check IsInList<T>(this Check check, IEnumerable<T> set, T search) => check.Resolve(set.Contains(search));

        public static Check IsNotInList<T>(this Check check, IEnumerable<T> set, T search) => check.Resolve(!set.Contains(search));

        public static Check IsBase64String(this Check check, string value) => check.Resolve(Convert.TryFromBase64String(value, new Span<byte>(new byte[value.Length]), out _));

        public static Check IsNotBase64String(this Check check, string value) => !check.Resolve(Convert.TryFromBase64String(value, new Span<byte>(new byte[value.Length]), out _));

        public static Check IsValidEmailFormat(this Check check, string email) => IsValidEmailFormatInternal(check, email);

        public static Check IsNotValidEmailFormat(this Check check, string email) => !IsValidEmailFormatInternal(check, email);

        public static Check IsValidUrlFormat(this Check check, string url) => check.Resolve(Uri.IsWellFormedUriString(url, UriKind.Absolute));

        public static Check IsNotValidUrlFormat(this Check check, string url) => !check.Resolve(Uri.IsWellFormedUriString(url, UriKind.Absolute));

        public static Check IsValidPhoneFormat(this Check check, string phone) => check.Resolve(RegexCatalog.Phone().IsMatch(phone));

        public static Check IsNotValidPhoneFormat(this Check check, string phone) => !check.Resolve(RegexCatalog.Phone().IsMatch(phone));

        public static Check IsEmpty(this Check check, Guid guid) => check.Resolve(guid == Guid.Empty);

        public static Check IsNotEmpty(this Check check, Guid guid) => check.Resolve(guid != Guid.Empty);

        public static Check IsValidIPAddress(this Check check, string ipAddress) => check.Resolve(RegexCatalog.IPAddressPattern().IsMatch(ipAddress));

        public static Check IsNotValidIPAddress(this Check check, string ipAddress) => !check.Resolve(RegexCatalog.IPAddressPattern().IsMatch(ipAddress));

        public static Check StartsWith(this Check check, string value, string search) => check.Resolve(value.StartsWith(search));

        public static Check DoesNotStartsWith(this Check check, string value, string search) => !check.Resolve(value.StartsWith(search));

        public static Check IsValidNameFormat(this Check check, string firstName, string lastName) => string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ? check.Resolve(false) : check.Resolve(true);

        public static Check IsValidUSPotalCode(this Check check, string postalCode) => check.Resolve(RegexCatalog.PostalCode().IsMatch(postalCode));

        public static Check IsNotValidUSPostalCode(this Check check, string postalCode) => !check.Resolve(RegexCatalog.PostalCode().IsMatch(postalCode));

        public static Check IsValidAddress(this Check check, string primaryAddress, string? city, USPostalCodeValue postalCode)
        {
            if (string.IsNullOrWhiteSpace(primaryAddress))
            {
                return check.Resolve(false);
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return check.Resolve(false);
            }

            try
            {
                return Check.For.IsValidUSPotalCode(postalCode);
            }
            catch (RegexMatchTimeoutException)
            {
                return check.Resolve(false);
            }
        }

        public static Check IsValidMoneyFormat(this Check check, decimal amount, CurrencyValue currency)
        {
            if (decimal.Round(amount, currency.MinorUnits) != amount)
            {
                return check.Resolve(false);
            }

            try
            {
                return check.GreaterThanOrEqual(amount, 0);
            }
            catch (KernelErrorException)
            {
                return check.Resolve(false);
            }
        }

        public static Check IsValidCssColor(this Check check, string cssColor)
        {
            var normalizedValue = cssColor.Trim().ToLower();

            return check.Resolve(normalizedValue.StartsWith('#')
                ? RegexCatalog.CssColor().IsMatch(normalizedValue)
                : _validColorNames.Contains(normalizedValue));
        }

        private static Check IsValidEmailFormatInternal(Check check, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return check.Resolve(false);
            }

            try
            {
                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }

                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            }
            catch (RegexMatchTimeoutException)
            {
                return check.Resolve(false);
            }
            catch (ArgumentException)
            {
                return check.Resolve(false);
            }

            try
            {
                return check.Resolve(Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            }
            catch (RegexMatchTimeoutException)
            {
                return check.Resolve(false);
            }
        }

        private static readonly string[] _validColorNames = {
            "aliceblue",
            "antiquewhite",
            "aqua",
            "aquamarine",
            "azure",
            "beige",
            "bisque",
            "black",
            "blanchedalmond",
            "blue",
            "blueviolet",
            "brown",
            "burlywood",
            "cadetblue",
            "chartreuse",
            "chocolate",
            "coral",
            "cornflowerblue",
            "cornsilk",
            "crimson",
            "cyan",
            "darkblue",
            "darkcyan",
            "darkgoldenrod",
            "darkgray",
            "darkgrey",
            "darkgreen",
            "darkkhaki",
            "darkmagenta",
            "darkolivegreen",
            "darkorange",
            "darkorchid",
            "darkred",
            "darksalmon",
            "darkseagreen",
            "darkslateblue",
            "darkslategray",
            "darkslategrey",
            "darkturquoise",
            "darkviolet",
            "deeppink",
            "deepskyblue",
            "dimgray",
            "dimgrey",
            "dodgerblue",
            "firebrick",
            "floralwhite",
            "forestgreen",
            "fuchsia",
            "gainsboro",
            "ghostwhite",
            "gold",
            "goldenrod",
            "gray",
            "grey",
            "green",
            "greenyellow",
            "honeydew",
            "hotpink",
            "indianred",
            "indigo",
            "ivory",
            "khaki",
            "lavender",
            "lavenderblush",
            "lawngreen",
            "lemonchiffon",
            "lightblue",
            "lightcoral",
            "lightcyan",
            "lightgoldenrodyellow",
            "lightgray",
            "lightgrey",
            "lightgreen",
            "lightpink",
            "lightsalmon",
            "lightseagreen",
            "lightskyblue",
            "lightslategray",
            "lightslategrey",
            "lightsteelblue",
            "lightyellow",
            "lime",
            "limegreen",
            "linen",
            "magenta",
            "maroon",
            "mediumaquamarine",
            "mediumblue",
            "mediumorchid",
            "mediumpurple",
            "mediumseagreen",
            "mediumslateblue",
            "mediumspringgreen",
            "mediumturquoise",
            "mediumvioletred",
            "midnightblue",
            "mintcream",
            "mistyrose",
            "moccasin",
            "navajowhite",
            "navy",
            "oldlace",
            "olive",
            "olivedrab",
            "orange",
            "orangered",
            "orchid",
            "palegoldenrod",
            "palegreen",
            "paleturquoise",
            "palevioletred",
            "papayawhip",
            "peachpuff",
            "peru",
            "pink",
            "plum",
            "powderblue",
            "purple",
            "rebeccapurple",
            "red",
            "rosybrown",
            "royalblue",
            "saddlebrown",
            "salmon",
            "sandybrown",
            "seagreen",
            "seashell",
            "sienna",
            "silver",
            "skyblue",
            "slateblue",
            "slategray",
            "slategrey",
            "snow",
            "springgreen",
            "steelblue",
            "tan",
            "teal",
            "thistle",
            "tomato",
            "turquoise",
            "violet",
            "wheat",
            "white",
            "whitesmoke",
            "yellow",
            "yellowgreen"};
    }
}
