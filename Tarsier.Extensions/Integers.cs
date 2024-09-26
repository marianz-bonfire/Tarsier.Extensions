using System;
using System.Text.RegularExpressions;

namespace Tarsier.Extensions
{
    public static class Integers
    {
        public static string ToNumericOnly(this string input) {
            Regex rgx = new Regex("[^0-9]");
            return rgx.Replace(input, string.Empty);
        }
        public static int ToSafeInteger(this object integerValue) {
            return integerValue.ToSafeString().ToSafeInteger();
        }
        public static int ToSafeInteger(this string integerValue) {
            if (String.IsNullOrEmpty(integerValue)) {
                return 0;
            }
            int outInt = 0;
            if (int.TryParse(integerValue.Trim(), out outInt)) {
                return outInt;
            }
            return outInt;
        }

        public static long ToSafeLong(this object int64Value) {
            return int64Value.ToSafeString().ToSafeLong();
        }

        public static long ToSafeLong(this string int64Value) {
            if (string.IsNullOrEmpty(int64Value.Trim())) {
                return 0;
            }

            long outLong = 0;
            try {
                outLong = Convert.ToInt64(int64Value);
            } catch {
                if (long.TryParse(int64Value.Trim(), out outLong)) {
                    return outLong;
                }
            }
            return outLong;
        }
    }
}
