using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace Tarsier.Extensions
{
    public static class Objects
    {
        public static string GetTableName<T>(this T entity) {
            if (entity != null) {
                return entity.GetType().Name.RemoveNonAlphaNumeric();
            }
            return string.Empty;
        }

        public static T Deserialize<T>(this string stringValue) {
            T temp = default(T);
            try {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(stringValue)) {
                    temp = (T)xml.Deserialize(reader);
                }
            } catch { /*@DO NOTHING */ }
            return temp;
        }

        public static string Serialize<T>(this T entity) {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            string serializeValue = string.Empty;
            try {
                using (StringWriter writer = new StringWriter()) {
                    xml.Serialize(writer, entity);
                    serializeValue = writer.ToString();
                }
            } catch { }
            return serializeValue;
        }

        public static T ParseData<T>(dynamic value) {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }
    }
}
