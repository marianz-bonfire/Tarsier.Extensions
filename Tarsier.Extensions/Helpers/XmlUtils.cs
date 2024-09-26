using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Tarsier.Extensions.Helpers
{
    public static class XmlUtils
    {
        public static XmlNamespaceManager CreateXmlNamespaceManager(XmlDocument doc, string defaultNamespace) {
            XmlNamespaceManager xmlNamespaceManagers = new XmlNamespaceManager(doc.NameTable);
            foreach (XmlAttribute attribute in doc.DocumentElement.Attributes) {
                if (attribute.Prefix == "xmlns") {
                    xmlNamespaceManagers.AddNamespace(attribute.LocalName, attribute.Value);
                }
                if (attribute.Name != "xmlns") {
                    continue;
                }
                xmlNamespaceManagers.AddNamespace(defaultNamespace, attribute.Value);
            }
            return xmlNamespaceManagers;
        }

        public static bool? GetXmlAttributeBool(XmlNode node, string attributeName) {
            string xmlAttributeString = XmlUtils.GetXmlAttributeString(node, attributeName);
            if (xmlAttributeString == null) {
                return null;
            }
            return new bool?(XmlConvert.ToBoolean(xmlAttributeString));
        }

        public static int GetXmlAttributeInt(XmlNode node, string attributeName, int defaultValue) {
            string xmlAttributeString = XmlUtils.GetXmlAttributeString(node, attributeName);
            if (xmlAttributeString == null) {
                return defaultValue;
            }
            return XmlConvert.ToInt32(xmlAttributeString);
        }

        public static string GetXmlAttributeString(XmlNode node, string attributeName) {
            XmlAttribute itemOf = node.Attributes[attributeName];
            if (itemOf == null) {
                return null;
            }
            return itemOf.InnerText;
        }

        public static bool GetXmlBool(XmlNode node, string xPath, XmlNamespaceManager ns = null) {
            string xmlString = XmlUtils.GetXmlString(node, xPath, ns);
            if (xmlString == null) {
                return false;
            }
            if (!(xmlString == "1") && !(xmlString == "true") && !(xmlString == "True")) {
                return false;
            }
            return true;
        }

        public static DateTime GetXmlDateTime(XmlNode node, string xPath, XmlNamespaceManager ns = null) {
            DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            string xmlString = XmlUtils.GetXmlString(node, xPath, ns);
            if (xmlString == null) {
                return dateTime;
            }
            try {
                dateTime = XmlConvert.ToDateTime(xmlString, XmlDateTimeSerializationMode.Utc);
            } catch {
            }
            return dateTime;
        }

        public static decimal GetXmlDecimal(XmlNode node, string XPath, XmlNamespaceManager ns = null) {
            string xmlString = XmlUtils.GetXmlString(node, XPath, ns);
            if (xmlString == null) {
                return decimal.Zero;
            }
            decimal num = new decimal();
            decimal.TryParse(xmlString, NumberStyles.Any, CultureInfo.InvariantCulture, out num);
            return num;
        }

        public static T GetXmlEnum<T>(XmlNode node, string xPath, XmlNamespaceManager ns = null) {
            string xmlString = XmlUtils.GetXmlString(node, xPath, ns);
            if (string.IsNullOrEmpty(xmlString)) {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), xmlString, true);
        }

        public static int GetXmlInt(XmlNode node, string XPath, XmlNamespaceManager ns = null) {
            string xmlString = XmlUtils.GetXmlString(node, XPath, ns);
            if (xmlString == null) {
                return 0;
            }
            int num = 0;
            int.TryParse(xmlString, out num);
            return num;
        }

        public static XmlNode GetXmlNode(XmlNode node, string xPath, XmlNamespaceManager ns = null) {
            return node.SelectSingleNode(xPath, ns);
        }

        public static string GetXmlString(XmlNode node, string xPath = null, XmlNamespaceManager ns = null) {
            if (node == null) {
                return null;
            }
            if (string.IsNullOrEmpty(xPath)) {
                if (node == null) {
                    return null;
                }
                return node.InnerText;
            }
            XmlNode xmlNodes = node.SelectSingleNode(xPath, ns);
            if (xmlNodes == null) {
                return null;
            }
            return xmlNodes.InnerText;
        }

        public static string MapTypeToXmlType(Type type) {
            if (type == null) {
                return null;
            }
            if (type == typeof(string) || type == typeof(char)) {
                return "string";
            }
            if (type == typeof(int) || type == typeof(int)) {
                return "integer";
            }
            if (type == typeof(short) || type == typeof(byte)) {
                return "short";
            }
            if (type == typeof(long) || type == typeof(long)) {
                return "long";
            }
            if (type == typeof(bool)) {
                return "boolean";
            }
            if (type == typeof(DateTime)) {
                return "datetime";
            }
            if (type == typeof(float)) {
                return "float";
            }
            if (type == typeof(decimal)) {
                return "decimal";
            }
            if (type == typeof(double)) {
                return "double";
            }
            if (type == typeof(float)) {
                return "single";
            }
            if (type == typeof(byte)) {
                return "byte";
            }
            if (type == typeof(byte[])) {
                return "base64Binary";
            }
            return null;
        }

        public static Type MapXmlTypeToType(string xmlType) {
            xmlType = xmlType.ToLower();
            if (xmlType == "string") {
                return typeof(string);
            }
            if (xmlType == "integer") {
                return typeof(int);
            }
            if (xmlType == "long") {
                return typeof(long);
            }
            if (xmlType == "boolean") {
                return typeof(bool);
            }
            if (xmlType == "datetime") {
                return typeof(DateTime);
            }
            if (xmlType == "float") {
                return typeof(float);
            }
            if (xmlType == "decimal") {
                return typeof(decimal);
            }
            if (xmlType == "double") {
                return typeof(double);
            }
            if (xmlType == "single") {
                return typeof(float);
            }
            if (xmlType == "byte") {
                return typeof(byte);
            }
            if (xmlType != "base64binary") {
                return null;
            }
            return typeof(byte[]);
        }

        public static string XmlString(string text, bool isAttribute = false) {
            if (string.IsNullOrEmpty(text)) {
                return text;
            }
            StringBuilder stringBuilder = new StringBuilder(text.Length);
            string str = text;
            for (int i = 0; i < str.Length; i++) {
                char chr = str[i];
                if (chr == '<') {
                    stringBuilder.Append("&lt;");
                } else if (chr == '>') {
                    stringBuilder.Append("&gt;");
                } else if (chr == '&') {
                    stringBuilder.Append("&amp;");
                } else if (isAttribute && chr == '\"') {
                    stringBuilder.Append("&quot;");
                } else if (isAttribute && chr == '\'') {
                    stringBuilder.Append("&apos;");
                } else if (chr == '\n') {
                    stringBuilder.Append((isAttribute ? "&#xA;" : "\n"));
                } else if (chr == '\r') {
                    stringBuilder.Append((isAttribute ? "&#xD;" : "\r"));
                } else if (chr != '\t') {
                    if (chr < ' ') {
                        short num = Convert.ToInt16(chr);
                        throw new InvalidOperationException(string.Concat("Invalid character in Xml String. Chr ", num.ToString(), " is illegal."));
                    }
                    stringBuilder.Append(chr);
                } else {
                    stringBuilder.Append((isAttribute ? "&#x9;" : "\t"));
                }
            }
            return stringBuilder.ToString();
        }
    }
}