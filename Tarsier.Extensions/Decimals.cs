using System;

namespace Tarsier.Extensions
{
    public static class Decimals
    {
        public static decimal ToSafeDecimal(this object decimalValue) {
            return decimalValue.ToSafeString().ToSafeDecimal();
        }
        public static decimal ToSafeDecimal(this string decimalValue) {
            if (string.IsNullOrWhiteSpace(decimalValue)) {
                return 0;
            }

            decimal outDecimal = 0;
            try {
                outDecimal = Convert.ToDecimal(decimalValue);
            } catch {
                if (decimal.TryParse(decimalValue.Trim(), out outDecimal)) {
                    return outDecimal;
                }
            }
            return outDecimal;
        }

    }
}
