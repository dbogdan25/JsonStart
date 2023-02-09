using System;
using System.Globalization;

namespace Json
{
    public static class JsonString
    {
        public static string Quoted(string input)
        {
            return "\"" + input.Replace("\"", "\\\"") + "\"";
        }

        public static bool IsJsonString(string input)
        {
            if (!IsValidString(input))
            {
                return false;
            }

            if (ContainsNewLineOrFormFeed(input))
            {
                return false;
            }

            if (ContainsInvalidHexEscape(input))
            {
                return false;
            }

            if (EndsWithEscape(input))
            {
                return false;
            }

            if (ContainsInvalidUnicodeEscape(input))
            {
                return false;
            }

            return true;
        }

        private static bool IsValidString(string input)
        {
            return !string.IsNullOrEmpty(input) && input.StartsWith("\"") && input.EndsWith("\"");
        }

        private static bool ContainsNewLineOrFormFeed(string input)
        {
            return input.Contains("\n") || input.Contains("\r") || input.Contains("\f");
        }

        private static bool ContainsInvalidHexEscape(string input)
        {
            return input.Contains(@"\x");
        }

        private static bool EndsWithEscape(string input)
        {
            const int a = 2;
            return input.Length > 1 && input[input.Length - a] == '\\';
        }

        private static bool ContainsInvalidUnicodeEscape(string input)
        {
            const int b = 5;
            const int c = 6;
            var escapeIndex = input.IndexOf(@"\u");
            while (escapeIndex >= 0 && escapeIndex + b < input.Length)
            {
                var escape = input.Substring(escapeIndex + 2, 4);
                var isHex = int.TryParse(escape, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _);
                if (!isHex)
                {
                    return true;
                }

                escapeIndex = input.IndexOf(@"\u", escapeIndex + c);
            }

            return escapeIndex >= 0;
        }
    }
}
