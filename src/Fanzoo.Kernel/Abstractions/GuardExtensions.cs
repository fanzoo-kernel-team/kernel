#pragma warning disable IDE0060 // Remove unused parameter
using System.Globalization;

namespace Fanzoo.Kernel
{
    public static class GuardExtensions
    {
        public static void Null<T>(this Guard guard, T value, string argument) => _ = value ?? throw new ArgumentNullException(argument);

        public static void NullOrWhiteSpace(this Guard guard, string value, string argument)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void ExceedsMaxValue(this Guard guard, int value, int maximum, string argument)
        {
            if (value > maximum)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void ExceedsMaxValue(this Guard guard, long value, long maximum, string argument)
        {
            if (value > maximum)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LengthLessThan(this Guard guard, string value, int minValue, string argument)
        {
            if (value.Length <= minValue)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LengthGreaterThan(this Guard guard, string value, int maxValue, string argument)
        {
            if (value.Length >= maxValue)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LessThan(this Guard guard, int value, int minValue, string argument)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LessThanMinCurrency(this Guard guard, decimal value, decimal minValue, string argument)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void InvalidMinorUnits(this Guard guard, decimal value, int minorUnits, string argument)
        {
            if (decimal.Round(value, minorUnits) != value)
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void NonMatchingRegex(this Guard guard, string value, string pattern, string argument)
        {
            if (!Regex.IsMatch(value, pattern))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void NotInList<T>(this Guard guard, IEnumerable<T> set, T search, string argument)
        {
            if (!set.Contains(search))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void InvalidBase64String(this Guard guard, string value, string argument)
        {
            if (!Convert.TryFromBase64String(value, new Span<byte>(new byte[value.Length]), out _))
            {
                throw new ArgumentException(argument);
            }
        }
        
        public static void InvalidPrefix(this Guard guard, string  value, string checkFor, string argument)
        {
            if (!value.StartsWith(checkFor))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidEmailFormat(this Guard guard, string value, string argument)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(argument);
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
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            }
            catch (RegexMatchTimeoutException)
            {
                throw new ArgumentException(argument);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException(argument);
            }

            try
            {
                var result = Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                if (!result)
                {
                    throw new ArgumentException(argument);
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new ArgumentException(argument);
            }
        }
    }
}
