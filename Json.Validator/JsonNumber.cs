using System;
using System.Globalization;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (!IsValidStart(input, input[0]))
            {
                return false;
            }

            if (EndsWithADot(input))
            {
                return false;
            }

            if (!IsValidNumberFormat(input))
            {
                return false;
            }

            if (input == "-0")
            {
                return true;
            }

            return double.TryParse(input, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out double result);
        }

        private static bool IsValidStart(string input, char c)
        {
            if (c == '-' || c == '+')
            {
                return IsValidStart(input.Substring(1), input[1]);
            }
            else if (c == '0' && input.Length > 1 && input[1] != '.')
            {
                return false;
            }

            return true;
        }

        private static bool EndsWithADot(string input)
        {
            return input.Length > 1 && input.EndsWith(".");
        }

        private static bool IsValidExponentFormat(string input, int index)
        {
            int length = input.Length;
            const int v = 2;
            if (index + 1 >= length)
            {
                return false;
            }

            char nextChar = input[index + 1];
            if (nextChar == '+' || nextChar == '-')
            {
                if (index + v >= length)
                {
                    return false;
                }

                index++;
            }

            for (int i = index + 1; i < length; i++)
            {
                if (!char.IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidNumberFormat(string input)
        {
            bool containsDecimalPoint = false;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '.')
                {
                    if (containsDecimalPoint)
                    {
                        return false;
                    }

                    containsDecimalPoint = true;
                }
                else if (c == 'e' || c == 'E')
                {
                    if (!IsValidExponentFormat(input, i))
                    {
                        return false;
                    }
                }
                else if (!char.IsDigit(c) && c != '+' && c != '-')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
