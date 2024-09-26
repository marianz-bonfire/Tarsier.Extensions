using System;

namespace Tarsier.Extensions
{
    public static class Booleans
    {
        public static bool ToSafeBoolean(this object toBool) {
            return toBool.ToSafeString().ToSafeBoolean();
        }
        public static bool ToSafeBoolean(this int toBool) {
            return toBool.ToSafeString().ToSafeBoolean();
        }
        public static bool ToSafeBoolean(this string toBool) {
            if (string.IsNullOrEmpty(toBool)) {
                return false;
            }
            string cleanedString = toBool.ToSafeString().ToLower();
            try {
                return Convert.ToBoolean(cleanedString);
            } catch {
                return (cleanedString.Equals("yes") ||
              cleanedString.Equals("y") ||
              cleanedString.Equals("true") ||
              cleanedString.Equals("t") ||
              cleanedString.Equals("1"));
            }

        }
    }
}
