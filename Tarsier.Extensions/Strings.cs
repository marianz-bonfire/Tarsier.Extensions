using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Tarsier.Extensions
{
    public static class Strings
    {
        public static String ToSafeString(this object obj) {
            return obj != null ? Convert.ToString(obj).Trim() : String.Empty;
        }

        public static String ToStringType(this String value) {
            return String.Format("'{0}'", value.EscapeQuote());
        }

        public static string ToTriggerString(this string argValue, string defaultValue) {
            return string.IsNullOrEmpty(argValue.ToSafeString()) ? defaultValue : argValue;
        }

        public static String EscapeQuote(this String value) {
            value = value.ToSafeString().Trim();
            if (!(String.IsNullOrEmpty(value))) {
                return value.Replace("'", "''");
            }
            return String.Empty;
        }

        public static string RemoveNonAlphaNumeric(this object value) {
            return value.ToSafeString().RemoveNonAlphaNumeric();
        }

        public static string RemoveNonAlphaNumeric(this string value) {
            Regex regex = new Regex("[^a-zA-Z0-9]");
            return regex.Replace(value.ToSafeString(), string.Empty);
        }

        public static string TrimDashes(this string value) {
            if (String.IsNullOrEmpty(value)) {
                return String.Empty;
            }
            return Regex.Replace(value.Trim(), "-{2,}", "-");
        }

        public static string TrimUnderscore(this string value) {
            if (String.IsNullOrEmpty(value)) {
                return String.Empty;
            }
            return Regex.Replace(value.Trim(), "_{2,}", "_");
        }
        public static string ReplaceUnderscore(this string value) {
            if (String.IsNullOrEmpty(value)) {
                return String.Empty;
            }
            return Regex.Replace(value.TrimDashes(), "-", "_");
        }
        public static string RemoveSpaces(this string value) {
            if (String.IsNullOrEmpty(value)) {
                return String.Empty;
            }
            return value.Trim().Replace(" ", "");
        }

        public static string ParseDollarSign(this string sheetName) {
            if (String.IsNullOrEmpty(sheetName)) {
                return String.Empty;
            }
            string[] array = sheetName.Split(new char[] { '$' });
            return array[0];
        }

        public static string ToAlphaOnly(this string input) {
            Regex rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, "");
        }

        public static string ToNumericOnly(this string input) {
            Regex rgx = new Regex("[^0-9]");
            return rgx.Replace(input, "");
        }

        public static string GetTabs(this int count) {
            return RepeatChar('\t', count);
        }

        public static string GetSpaces(this int count) {
            return RepeatChar(' ', count);
        }

        public static string RepeatChar(this char character, int count) {
            count = (count < 0) ? 0 : count;
            return new String(character, count);
        }

        public static string TruncatePath(this string path) {
            if (Regex.IsMatch(path, "^(\\w+:|\\\\)(\\\\[^\\\\]+\\\\).*(\\\\[^\\\\]+\\\\[^\\\\]+)$"))
                return Regex.Replace(path, "^(\\w+:|\\\\)(\\\\[^\\\\]+\\\\).*(\\\\[^\\\\]+\\\\[^\\\\]+)$", "$1$2...$3");
            return path;
        }

        public static string F(this string format, params object[] args) {
            if (args == null || !args.Any())
                return format;

            return String.Format(format, args);
        }

        public static bool IsNullOrEmpty(this string source) {
            return String.IsNullOrEmpty(source);
        }

        public static bool HasLetter(this string source) {
            var hasLetter = Regex.Matches(source, @"[a-zA-Z]").Count;
            if (hasLetter > 0) {
                return true;
            }
            return false;
        }

        public static bool ContainsNonPrintableCharacters(this string source) {
            for (int i = 0; i < source.Length; i++) {
                char c = source[i];
                UnicodeCategory[] nonRenderingCategories = new UnicodeCategory[] {
                    UnicodeCategory.Control,
                    UnicodeCategory.OtherNotAssigned,
                    UnicodeCategory.Surrogate
                };
                if ((char.IsWhiteSpace(c) ? false : nonRenderingCategories.Contains<UnicodeCategory>(char.GetUnicodeCategory(c)))) {
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsSpecialCharacters(this string source) {
            return !Regex.IsMatch(source, "^[a-zA-Z0-9]+$");
        }

        public static bool IsValidEmailAddress(this string value) {
            return (new Regex("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled)).IsMatch(value);
        }
        public static string Join(this IEnumerable<string> list) {
            return list == null ? "" : String.Join("", list);
        }

        public static string JoinWithSeparator(this IEnumerable<string> list, string separator) {
            return list == null ? "" : String.Join(separator, list);
        }

        public static string JoinWithComma(this IEnumerable<string> list) {
            return list == null ? "" : String.Join(",", list);
        }

        public static IEnumerable<string> Split(this string @value, int length) {
            if (String.IsNullOrEmpty(@value) || length < 1) {
                throw new ArgumentException();
            }
            return Enumerable.Range(0, value.Length / length).Select(i => value.Substring(i * length, length));
        }
        public static string CamelCaseToUnderscore(this string stringValue, bool lowerCase = true) {
            StringBuilder underscoreConventionBuilder = new StringBuilder();
            bool isStartOfWord = true;
            foreach (char ch in stringValue) {
                if (char.IsUpper(ch))
                    if (!isStartOfWord) underscoreConventionBuilder.Append("_");

                underscoreConventionBuilder.Append(ch.ToString().ToUpper());

                isStartOfWord = char.IsWhiteSpace(ch);
            }
            return lowerCase ? underscoreConventionBuilder.ToString().ToLower() : underscoreConventionBuilder.ToString().ToUpper();
        }

        public static string ToUnderscoreCase(this string str) {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string PluralizeLastWord(this string stringValue, string separator) {
            Console.WriteLine(stringValue);
            CultureInfo ci = new CultureInfo("en-us");
            PluralizationService ps = PluralizationService.CreateService(ci);
            string[] chunks = stringValue.Split(new char[] { Convert.ToChar(separator) });
            List<string> words = new List<string>();
            if (chunks.Length > 1) {
                for (int index = 0; index < chunks.Length; index++) {
                    string word = chunks[index];
                    if (index == chunks.Length - 1) {
                        if (ps.IsSingular(word) == true) {
                            word = ps.Pluralize(word);
                        }
                        words.Add(word);
                    } else {
                        words.Add(word);
                    }
                }
            } else {
                string word = chunks[0];
                if (ps.IsSingular(word) == true) {
                    word = ps.Pluralize(word);
                }
                return word;
            }
            return words.JoinWithSeparator("_");
        }

        public static string ToTitleCase(this string stringValue) {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            // Convert the input string to Title Case
            return textInfo.ToTitleCase(stringValue);
        }
    }
}

