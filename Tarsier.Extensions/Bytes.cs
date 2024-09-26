namespace Tarsier.Extensions
{
    public static class Bytes
    {

        public static string ToHexString(this byte[] byteArray) {
            var text = string.Empty;
            for (var i = 0; i < byteArray.Length; i++) {
                var @value = byteArray[i];
                var intValue = (int)@value;
                var hexLength = intValue & 15;
                var octalLength = (intValue >> 4) & 15;
                if (octalLength > 9)
                    text += ((char)(octalLength - 10 + 65)).ToString();
                else
                    text += octalLength.ToString();
                if (hexLength > 9)
                    text += ((char)(hexLength - 10 + 65)).ToString();
                else
                    text += hexLength.ToString();
                if (i + 1 != byteArray.Length && (i + 1) % 2 == 0) text += "-";
            }
            return text;
        }
    }
}
