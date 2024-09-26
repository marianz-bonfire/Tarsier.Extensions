using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using Tarsier.Extensions.Helpers;

namespace Tarsier.Extensions
{
    public static class Dictionaries
    {
        public static void FromXml(this IDictionary items, string xml) {
            items.Clear();
            foreach (XElement xElement in XElement.Parse(xml).Elements()) {
                string value = null;
                XAttribute xAttribute = xElement.Attribute("type");
                if (xAttribute != null) {
                    value = xAttribute.Value;
                }
                string str = xElement.Value;
                if (!string.IsNullOrEmpty(value) && value != "string" && !value.StartsWith("__")) {
                    Type type = XmlUtils.MapXmlTypeToType(value);
                    if (type == null) {
                        items.Add(xElement.Name.LocalName, str);
                    } else {
                        items.Add(xElement.Name.LocalName, Reflections.StringToTypedValue(str, type, null));
                    }
                } else if (!value.StartsWith("___")) {
                    items.Add(xElement.Name.LocalName, str);
                } else {
                    Type typeFromName = Reflections.GetTypeFromName(value.Substring(3));
                    object obj = Serializations.DeSerializeObject(xElement.Elements().First<XElement>().CreateReader(), typeFromName);
                    items.Add(xElement.Name.LocalName, obj);
                }
            }
        }

        public static string ToXml(this IDictionary items, string root = "root") {
            XElement xElement = new XElement(root);
            foreach (DictionaryEntry item in items) {
                string xmlType = XmlUtils.MapTypeToXmlType(item.Value.GetType());
                XAttribute xAttribute = null;
                if (string.IsNullOrEmpty(xmlType)) {
                    string str = null;
                    if (!Serializations.SerializeObject(item.Value, out str)) {
                        continue;
                    }
                    XElement xElement1 = XElement.Parse(str);
                    xElement.Add(new XElement(item.Key as string, new object[] { new XAttribute("type", string.Concat("___", item.Value.GetType().FullName)), xElement1 }));
                } else {
                    xAttribute = new XAttribute("type", xmlType);
                    xElement.Add(new XElement(item.Key as string, new object[] { xAttribute, item.Value }));
                }
            }
            return xElement.ToString();
        }
    }
}
