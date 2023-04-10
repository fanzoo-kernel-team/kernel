#pragma warning disable IDE0060 // Remove unused parameter


namespace Fanzoo.Kernel
{
    public static class GuardExtensions
    {

        public static void Null<T>(this Guard guard, T value, string argument) => _ = value ?? throw new ArgumentNullException(argument);

        public static void NullOrWhiteSpace(this Guard guard, string value, string argument)
        {
            if (Check.For.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(argument);
            }
        }

        public static void ExceedsMaxValue(this Guard guard, int value, int maximum, string argument)
        {
            if (Check.For.GreaterThan(value, maximum))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void ExceedsMaxValue(this Guard guard, long value, long maximum, string argument)
        {
            if (Check.For.GreaterThan(value, maximum))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LengthLessThan(this Guard guard, string value, int minValue, string argument)
        {
            if (Check.For.LengthIsLessThan(value, minValue))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LengthGreaterThan(this Guard guard, string value, int maxValue, string argument)
        {
            if (Check.For.LengthIsGreaterThan(value, maxValue))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LessThan(this Guard guard, int value, int minValue, string argument)
        {
            if (Check.For.LessThan(value, minValue))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void LessThan(this Guard guard, decimal value, decimal minValue, string argument)
        {
            if (Check.For.LessThan(value, minValue))
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
            if (Check.For.DoesNotMatch(value, pattern))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void NotInList<T>(this Guard guard, IEnumerable<T> set, T search, string argument)
        {
            if (Check.For.IsNotInList(set, search))
            {
                throw new ArgumentOutOfRangeException(argument);
            }
        }

        public static void InvalidBase64String(this Guard guard, string value, string argument)
        {
            if (Check.For.IsNotBase64String(value))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidPrefix(this Guard guard, string value, string prefix, string argument)
        {
            if (Check.For.DoesNotStartsWith(value, prefix))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidEmailFormat(this Guard guard, string value, string argument)
        {
            if (Check.For.IsNotValidEmailFormat(value))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidUrlFormat(this Guard guard, string url, string argument)
        {
            if (Check.For.IsNotValidUrlFormat(url))
            {
                throw new UriFormatException();
            }
        }

        public static void InvalidPhoneNumber(this Guard guard, string value, string argument)
        {
            if (Check.For.IsNotValidPhoneFormat(value))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidUSPostalCode(this Guard guard, string value, string argument)
        {
            if (Check.For.IsNotValidUSPostalCode(value))
            {
                throw new ArgumentException(argument);
            }
        }

        public static void InvalidPassword(this Guard guard, string value, string argument)
        {
            if (Check.For.IsValidPassword(value).Result is not true)
            {
                throw new ArgumentException(argument);
            }
        }
    }
}
