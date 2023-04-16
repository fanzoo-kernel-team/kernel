using System.Text;

namespace System
{
    public static class Extensions
    {
        public static string Join<T>(this IEnumerable<T> list, char separator) => string.Join(separator, list);

        public static string Format(this string value, string format) => string.Format(format, value);

        public static bool IsUnique(this string[] value) => value.GroupBy(s => s).Count() == value.Length;

        public static bool IsNullOrWhitespace(this string? value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNotNullOrWhitespace(this string? value) => !string.IsNullOrWhiteSpace(value);

        public static string ConvertToEmailName(this string value)
        {
            value = value.ToLower();

            value = new string(value.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

            value = RemoveWhitespace(value.Trim());

            value = value.Replace(" ", "-");

            return value;
        }

        public static string RemoveSpaces(this string value) => new(value.Where(c => !char.IsWhiteSpace(c)).ToArray());

        public static string RemoveWhiteSpace(this string value) => RemoveWhitespace(value);

        private static string RemoveWhitespace(string value) //https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
        {
            var dst = new char[value.Length];
            uint end = 0;
            var prev = char.MinValue;
            for (var k = 0; k < value.Length; ++k)
            {
                var c = value[k];
                dst[end] = c;

                // We'll move forward if the current character is not ' ' or if prev char is not ' '
                // To avoid 'if' let's get diffs for c and prev and then use bitwise operatios to get 
                // 0 if n is 0 or 1 if n is non-zero
                var x = (uint)(' ' - c) + (uint)(' ' - prev); // non zero if any non-zero

                end += ((x | (~x + 1)) >> 31) & 1; // https://stackoverflow.com/questions/3912112/check-if-a-number-is-non-zero-using-bitwise-operators-in-c by ruslik
                prev = c;
            }

            return new string(dst, 0, (int)end);
        }

        public static string? ValueFromStart(this string value, string endString) => ValueBetween(value, "", endString);

        public static string? ValueBetween(this string value, string startString, string endString)
        {
            var start = value.IndexOf(startString) + startString.Length;
            var end = value.IndexOf(endString, start);

            return start < 0 || end < 0 ? null : value[start..end];
        }

        public static string? ValueBetween(this string value, string startString, string endString, int startIndex)
        {
            if (startIndex < 0)
            {
                return null;
            }

            var start = value.IndexOf(startString, startIndex) + startString.Length;
            var end = value.IndexOf(endString, start);

            return value[start..end];
        }

        public static string RemoveWords(this string value, bool ignoreCase, params string[] words)
        {
            var testPhrase = value;
            var testWords = words;

            var sb = new StringBuilder();

            if (ignoreCase)
            {
                testPhrase = testPhrase.ToLower();

                for (var i = 0; i < testWords.Length; i++)
                {
                    testWords[i] = testWords[i].ToLower();
                }
            }

            var splitText = testPhrase.Trim().Split(' ');

            for (var i = 0; i < splitText.Length; i++)
            {
                var t = splitText[i];

                if (!string.IsNullOrWhiteSpace(t) && !testWords.Contains(t.ToAlphaNumeric()))
                {
                    sb.Append(value.Split(' ')[i] + " ");
                }
            }

            return sb.ToString().Trim();
        }

        public static string ToAlphaNumeric(this string value)
        {
            var sb = new StringBuilder();

            foreach (var c in value)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Replace("  ", " ");
        }

        public static string RemovePunctuation(this string value)
        {
            var sb = new StringBuilder();

            foreach (var c in value)
            {
                if (!char.IsPunctuation(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Replace("  ", " ");
        }

        public static bool StartsWithAny(this string value, params string[] args)
        {

            foreach (var s in args)
            {
                if (value.StartsWith(s))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToDigits(this string value)
        {
            var sb = new StringBuilder();

            foreach (var c in value.Where(char.IsDigit))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static byte[] ReadAllBytes(this Stream stream)
        {
            using var memory = new MemoryStream();

            stream.CopyTo(memory);

            return memory!.ToArray();
        }

        public static int ToInt32(this string? value) => Convert.ToInt32(value);

        public static void RemoveAll<T>(this IList<T> items, Func<T, bool> predicate)
        {
            if (items is List<T> list)
            {
                list.RemoveAll(new Predicate<T>(predicate));
            }
            else
            {
                var itemsToDelete = items.Where(predicate).ToArray();

                foreach (var item in itemsToDelete)
                {
                    items.Remove(item);
                }
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> destination, IDictionary<TKey, TValue> source)
        {
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }

        public static void AddRange<T>(this IList<T> destination, IList<T> source)
        {
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }
    }
}
