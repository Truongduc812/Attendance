using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace iSoft.Common.Utils
{
    public static class StringUtil
    {

        public static string Join(this string[] arr, string joinSeperator)
        {
            return string.Join(joinSeperator, arr);
        }

        /// <summary>
        /// GenerateRandomKeyWithTicks
        /// </summary>
        /// <returns>EX: 638340036329629866_28286595</returns>
        public static string GenerateRandomKeyWithTicks()
        {
            return DateTime.Now.Ticks.ToString() + "_" + NumberUtil.GetRandomInt(10000000, 99999999).ToString();
        }

        /// <summary>
        /// GenerateRandomKeyWithDateTime
        /// </summary>
        /// <returns>EX: 20231027_150750_28286595</returns>
        public static string GenerateRandomKeyWithDateTime()
        {
            return DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd_HHmmss") + "_" + NumberUtil.GetRandomInt(10000000, 99999999).ToString();
        }

        public static Guid GenerateGuidFromTicks(long ticks)
        {
            byte[] ticksBytes = BitConverter.GetBytes(ticks);
            byte[] randomBytes = new byte[10];
            new Random().NextBytes(randomBytes);

            byte[] combinedBytes = new byte[16];
            Array.Copy(ticksBytes, combinedBytes, 8);
            Array.Copy(randomBytes, 0, combinedBytes, 8, 8);

            return new Guid(combinedBytes);
        }
        public static Guid GenerateGuidFromTicks(DateTime date)
        {
            byte[] ticksBytes = BitConverter.GetBytes(date.ToOADate());
            byte[] randomBytes = new byte[8];
            new Random().NextBytes(randomBytes);

            byte[] combinedBytes = new byte[16];
            Array.Copy(ticksBytes, combinedBytes, 8);
            Array.Copy(randomBytes, 0, combinedBytes, 8, 8);

            return new Guid(combinedBytes);
        }

        public static string GetStringInPattern(string str, string pattern)
        {
            //string pattern = @"\{\{(.+?)\}\}";
            Match match = Regex.Match(str, pattern);

            if (match.Success)
            {
                string extractedString = match.Groups[1].Value;
                return extractedString;
            }
            return "";
        }

        public static bool IsMatch(string inputStr, string pattern)
        {
            return Regex.IsMatch(inputStr, pattern);
        }
        public static string Truncate(this string inputStr, int maxLength)
        {
            if (inputStr.Length <= maxLength)
            {
                return inputStr;
            }
            else
            {
                return inputStr.Substring(0, maxLength);
            }
        }
        public static (string fileName, string extension) SplitFileName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            return (fileName, extension);
        }
        public static string RemoveUnicode(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public static string RemoveSpecialChar(this string text, string escChar = @"\,")
        {
            string result = Regex.Replace(text, @"[^a-zA-Z0-9_" + escChar + @"]", "_");
            return result;
        }

        public static string ConvertToESField(this string text, long connectionId)
        {
            if (connectionId <= 0)
            {
                return text.RemoveSpecialChar().Trim().ToLower();
            }
            return $"{connectionId}_{text}".RemoveSpecialChar().Trim().ToLower();
        }

        public static string UpperFirstLetter(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string LowerFirstLetter(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string RemoveLastChar(this string str)
        {
            if (str.Length >= 1)
            {
                return str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public static double ToDoubleValue(this string text, double defaultValue)
        {
            double value;
            if (double.TryParse(text.Replace(',', '.').ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public static long ToLongValue(this string text, long defaultValue)
        {
            long value;
            if (long.TryParse(text, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return string.Concat(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                .Normalize(NormalizationForm.FormC);
        }
    }

}
