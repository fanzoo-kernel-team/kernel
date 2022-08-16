using System.Globalization;

namespace Fanzoo.Kernel
{
    public static class CheckExtensions
    {
        private const string PhonePattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        private const string IPAddressPattern = @"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";

        public static ICheckResult Null<T>(this Check check, T value) => check.Resolve(value is not null);

        public static ICheckResult NullOrWhiteSpace(this Check check, string value) => check.Resolve(!string.IsNullOrWhiteSpace(value));

        public static ICheckResult ExceedsMaxValue(this Check check, int value, int maximum) => check.Resolve(value <= maximum);

        public static ICheckResult ExceedsMaxValue(this Check check, long value, long maximum) => check.Resolve(value <= maximum);

        public static ICheckResult ExceedsMaxValue(this Check check, decimal value, decimal maximum) => check.Resolve(value <= maximum);

        public static ICheckResult LessThanMinValueValue(this Check check, int value, int minimum) => check.Resolve(value >= minimum);

        public static ICheckResult LessThanMinValueValue(this Check check, long value, long minimum) => check.Resolve(value >= minimum);

        public static ICheckResult LessThanMinValueValue(this Check check, decimal value, decimal minimum) => check.Resolve(value >= minimum);

        public static ICheckResult LengthExceedsMaximum(this Check check, string value, int maximum) => check.Resolve(value.Length <= maximum);

        public static ICheckResult LengthLessThanMinimum(this Check check, string value, int minimum) => check.Resolve(value.Length >= minimum);

        public static ICheckResult NonMatchingRegex(this Check check, string value, string pattern) => check.Resolve(Regex.IsMatch(value, pattern));

        public static ICheckResult NotInList<T>(this Check check, IEnumerable<T> set, T search) => check.Resolve(set.Contains(search));

        public static ICheckResult Base64String(this Check check, string value) => check.Resolve(Convert.TryFromBase64String(value, new Span<byte>(new byte[value.Length]), out _));

        public static ICheckResult ValidEmailFormat(this Check check, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return check.Resolve(false);
            }

            try
            {
                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
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

        public static ICheckResult ValidPhoneFormat(this Check check, string phone) => NonMatchingRegex(check, phone, PhonePattern);

        public static ICheckResult Empty(this Check check, Guid guid) => check.Resolve(guid != Guid.Empty);

        public static ICheckResult ValidIPAddress(this Check check, string ipAddress) => check.Resolve(Regex.IsMatch(ipAddress, IPAddressPattern));

        public static ICheckResult StartsWith(this Check check, string value, string search) => check.Resolve(value.StartsWith(search));
    }
}
