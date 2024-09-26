using System;

namespace Tarsier.Extensions
{
    public static class Dates
    {

        public static DateTime ToSafeDate(this object dateString) {
            return ToSafeDate(dateString.ToSafeString());
        }

        public static DateTime ToSafeDate(this string dateString) {
            DateTime dateTimeOut = default(DateTime);
            DateTime result = default(DateTime);
            if (DateTime.TryParse(dateString, out dateTimeOut)) {
                result = dateTimeOut;
            }
            return result;
        }

        public static string GetTimeFormatted(this String dateTime, bool ignoreInvalid = true) {
            if (ignoreInvalid) {
                if (dateTime.RemoveNonAlphaNumeric().ToSafeInteger() <= 0) {
                    return "--:--:-- ";
                }
            }
            return dateTime.ToSafeDate().GetTime();
        }

        public static string GetTime(this String dateTime) {
            return dateTime.ToSafeDate().GetTime();
        }

        public static string GetPlainDate(this object dateValue) {
            return dateValue.ToSafeDate().ToString("yyyyMMdd");
        }

        public static string GetTime(this DateTime dateTime, bool hasSeconds = true) {
            return dateTime.ToString(hasSeconds ? "hh:mm:ss tt" : "hh:mm tt");
        }

        public static bool IsGreaterThan(this DateTime thisFirstDate, DateTime compareToSecondDate) {
            int num = DateTime.Compare(thisFirstDate, compareToSecondDate);
            return num >= 0 && num != 0;
        }

        public static string GetForcedDatetime(this DateTime dateTime) {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
