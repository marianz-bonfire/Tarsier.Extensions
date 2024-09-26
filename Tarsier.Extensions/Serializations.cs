using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Tarsier.Extensions.Enums;

namespace Tarsier.Extensions
{
    public static class Serializations
    {
        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization) {
            return DeSerializeObject(fileName, objectType, binarySerialization, false);
        }

        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization, bool throwExceptions) {
            object deserializedObjectResult = null;
            if (binarySerialization) {
                BinaryFormatter binaryFormatter = null;
                FileStream fileStream = null;
                try {
                    try {
                        binaryFormatter = new BinaryFormatter();
                        fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        deserializedObjectResult = binaryFormatter.Deserialize(fileStream);
                    } catch {
                        deserializedObjectResult = null;
                    }
                } finally {
                    if (fileStream != null) {
                        fileStream.Close();
                    }
                }
            } else {
                XmlReader xmlTextReader = null;
                XmlSerializer xmlSerializer = null;
                FileStream fileStream1 = null;
                try {
                    try {
                        xmlSerializer = new XmlSerializer(objectType);
                        fileStream1 = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        xmlTextReader = new XmlTextReader(fileStream1);
                        deserializedObjectResult = xmlSerializer.Deserialize(xmlTextReader);
                    } catch (Exception exception) {
                        string message = exception.Message;
                        deserializedObjectResult = null;
                        if (throwExceptions) {
                            throw;
                        }
                    }
                } finally {
                    if (fileStream1 != null) {
                        fileStream1.Close();
                    }
                    if (xmlTextReader != null) {
                        xmlTextReader.Close();
                    }
                }
            }
            return deserializedObjectResult;
        }

        public static object DeSerializeObject(XmlReader reader, Type objectType) {
            object deserializedObjectResult = (new XmlSerializer(objectType)).Deserialize(reader);
            reader.Close();
            return deserializedObjectResult;
        }

        public static object DeSerializeObject(string xml, Type objectType) {
            return DeSerializeObject(new XmlTextReader(xml, XmlNodeType.Document, null), objectType);
        }

        public static object DeSerializeObject(byte[] buffer, Type objectType, bool throwExceptions = false) {
            object deserializedObjectResult = null;
            BinaryFormatter binaryFormatter = null;
            MemoryStream memoryStream = null;
            try {
                try {
                    binaryFormatter = new BinaryFormatter();
                    memoryStream = new MemoryStream(buffer);
                    deserializedObjectResult = binaryFormatter.Deserialize(memoryStream);
                } catch {
                    if (throwExceptions) {
                        throw;
                    }
                    deserializedObjectResult = null;
                }
            } finally {
                if (memoryStream != null) {
                    memoryStream.Close();
                }
            }
            return deserializedObjectResult;
        }

        public static string ObjectToString(object instanc, string separator, ObjectToStringTypes type) {
            FieldInfo[] fields = instanc.GetType().GetFields();
            string empty = string.Empty;
            if (type == ObjectToStringTypes.Properties || type == ObjectToStringTypes.PropertiesAndFields) {
                PropertyInfo[] properties = instanc.GetType().GetProperties();
                for (int i = 0; i < (int)properties.Length; i++) {
                    PropertyInfo propertyInfo = properties[i];
                    try {
                        empty = string.Concat(new string[] { empty, propertyInfo.Name, ":", propertyInfo.GetValue(instanc, null).ToString(), separator });
                    } catch {
                        empty = string.Concat(empty, propertyInfo.Name, ": n/a", separator);
                    }
                }
            }
            if (type == ObjectToStringTypes.Fields || type == ObjectToStringTypes.PropertiesAndFields) {
                FieldInfo[] fieldInfoArray = fields;
                for (int j = 0; j < (int)fieldInfoArray.Length; j++) {
                    FieldInfo fieldInfo = fieldInfoArray[j];
                    try {
                        empty = string.Concat(new string[] { empty, fieldInfo.Name, ": ", fieldInfo.GetValue(instanc).ToString(), separator });
                    } catch {
                        empty = string.Concat(empty, fieldInfo.Name, ": n/a", separator);
                    }
                }
            }
            return empty;
        }

        public static bool SerializeObject(object instance, string fileName, bool binarySerialization) {
            bool flag = true;
            if (binarySerialization) {
                Stream fileStream = null;
                try {
                    try {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        fileStream = new FileStream(fileName, FileMode.Create);
                        binaryFormatter.Serialize(fileStream, instance);
                    } catch {
                        flag = false;
                    }
                } finally {
                    if (fileStream != null) {
                        fileStream.Close();
                    }
                }
            } else {
                XmlTextWriter xmlTextWriter = null;
                try {
                    try {
                        XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
                        xmlTextWriter = new XmlTextWriter(new FileStream(fileName, FileMode.Create), new UTF8Encoding()) {
                            Formatting = Formatting.Indented,
                            IndentChar = ' ',
                            Indentation = 3
                        };
                        xmlSerializer.Serialize(xmlTextWriter, instance);
                    } catch (Exception exception) {
                        flag = false;
                    }
                } finally {
                    if (xmlTextWriter != null) {
                        xmlTextWriter.Close();
                    }
                }
            }
            return flag;
        }

        public static bool SerializeObject(object instance, XmlTextWriter writer, bool throwExceptions) {
            bool flag = true;
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = ' ';
                writer.Indentation = 3;
                xmlSerializer.Serialize(writer, instance);
            } catch (Exception exception) {
                if (throwExceptions) {
                    throw exception;
                }
                flag = false;
            }
            return flag;
        }

        public static bool SerializeObject(object instance, out string xmlResultString) {
            return SerializeObject(instance, out xmlResultString, false);
        }

        public static bool SerializeObject(object instance, out string xmlResultString, bool throwExceptions) {
            xmlResultString = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding());
            if (!SerializeObject(instance, xmlTextWriter, throwExceptions)) {
                memoryStream.Close();
                return false;
            }
            xmlResultString = Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            memoryStream.Close();
            xmlTextWriter.Close();
            return true;
        }

        public static bool SerializeObject(object instance, out byte[] resultBuffer, bool throwExceptions = false) {
            bool flag = true;
            MemoryStream memoryStream = null;
            try {
                try {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    memoryStream = new MemoryStream();
                    binaryFormatter.Serialize(memoryStream, instance);
                } catch (Exception exception) {
                    flag = false;
                    if (throwExceptions) {
                        throw exception;
                    }
                }
            } finally {
                if (memoryStream != null) {
                    memoryStream.Close();
                }
            }
            resultBuffer = memoryStream.ToArray();
            return flag;
        }

        public static byte[] SerializeObjectToByteArray(object instance, bool throwExceptions = false) {
            byte[] numArray = null;
            if (!SerializeObject(instance, out numArray, false)) {
                return null;
            }
            return numArray;
        }

        public static string SerializeObjectToString(object instance, bool throwExceptions = false) {
            string empty = string.Empty;
            if (!SerializeObject(instance, out empty, throwExceptions)) {
                return null;
            }
            return empty;
        }
    }
}