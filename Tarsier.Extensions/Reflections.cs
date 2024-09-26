using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Tarsier.Extensions
{
    public static class Reflections
    {
        public static object CallMethod(object instance, string method, Type[] parameterTypes, params object[] parms) {
            if (parameterTypes == null && parms.Length != 0) {
                return instance.GetType().GetMethod(method, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod).Invoke(instance, parms);
            }
            return instance.GetType().GetMethod(method, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, parameterTypes, null).Invoke(instance, parms);
        }

        public static object CallMethod(object instance, string method, params object[] parameters) {
            Type[] type = null;
            if (parameters != null) {
                type = new Type[(int)parameters.Length];
                int num = 0;
                while (num < (int)parameters.Length) {
                    if (parameters[num] != null) {
                        type[num] = parameters[num].GetType();
                        num++;
                    } else {
                        type = null;
                        break;
                    }
                }
            }
            return CallMethod(instance, method, type, parameters);
        }
        public static Type GetTypeFromName(string typeName, string assemblyName) {
            Type type = Type.GetType(typeName, false);
            if (type != null) {
                return type;
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < (int)assemblies.Length; i++) {
                type = assemblies[i].GetType(typeName, false);
                if (type != null) {
                    break;
                }
            }
            if (type != null) {
                return type;
            }
            if (!string.IsNullOrEmpty(assemblyName) && LoadAssembly(assemblyName) != null) {
                type = Type.GetType(typeName, false);
                if (type != null) {
                    return type;
                }
            }
            return null;
        }

        public static Type GetTypeFromName(string typeName) {
            return GetTypeFromName(typeName, null);
        }

        public static Assembly LoadAssembly(string assemblyName) {
            Assembly assembly = null;
            try {
                assembly = Assembly.Load(assemblyName);
            } catch {
            }
            if (assembly != null) {
                return assembly;
            }
            if (File.Exists(assemblyName)) {
                assembly = Assembly.LoadFrom(assemblyName);
                if (assembly != null) {
                    return assembly;
                }
            }
            return null;
        }

        public static object StringToTypedValue(string sourceString, Type targetType, CultureInfo culture = null) {
            object typedValue = null;
            bool flag = string.IsNullOrEmpty(sourceString);
            if (culture == null) {
                culture = CultureInfo.CurrentCulture;
            }
            if (targetType == typeof(string)) {
                typedValue = sourceString;
            } else if (targetType == typeof(int) || targetType == typeof(int)) {
                typedValue = (!flag ? int.Parse(sourceString, NumberStyles.Any, culture.NumberFormat) : 0);
            } else if (targetType == typeof(long)) {
                typedValue = (!flag ? long.Parse(sourceString, NumberStyles.Any, culture.NumberFormat) : (long)0);
            } else if (targetType == typeof(short)) {
                typedValue = (!flag ? short.Parse(sourceString, NumberStyles.Any, (IFormatProvider)culture.NumberFormat) : (short)0);
            } else if (targetType == typeof(decimal)) {
                typedValue = (!flag ? decimal.Parse(sourceString, NumberStyles.Any, culture.NumberFormat) : decimal.Zero);
            } else if (targetType == typeof(DateTime)) {
                typedValue = (!flag ? Convert.ToDateTime(sourceString, culture.DateTimeFormat) : DateTime.MinValue);
            } else if (targetType == typeof(byte)) {
                typedValue = (!flag ? Convert.ToByte(sourceString) : 0);
            } else if (targetType == typeof(double)) {
                typedValue = (!flag ? double.Parse(sourceString, NumberStyles.Any, (IFormatProvider)culture.NumberFormat) : 0f);
            } else if (targetType == typeof(float)) {
                typedValue = (!flag ? float.Parse(sourceString, NumberStyles.Any, (IFormatProvider)culture.NumberFormat) : 0f);
            } else if (targetType == typeof(bool)) {
                sourceString = sourceString.ToLower();
                typedValue = ((flag || !(sourceString == "true")) && !(sourceString == "on") && !(sourceString == "1") && !(sourceString == "yes") ? false : true);
            } else if (targetType == typeof(Guid)) {
                typedValue = (!flag ? new Guid(sourceString) : Guid.Empty);
            } else if (targetType.IsEnum) {
                typedValue = Enum.Parse(targetType, sourceString);
            } else if (targetType == typeof(byte[])) {
                typedValue = null;
            } else if (!targetType.Name.StartsWith("Nullable`")) {
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                if (converter == null || !converter.CanConvertFrom(typeof(string))) {
                    throw new InvalidCastException(string.Concat("Type Conversion failed for: ", targetType.Name));
                }
                typedValue = converter.ConvertFromString(null, culture, sourceString);
            } else if (sourceString.ToLower() == "null" || sourceString == string.Empty) {
                typedValue = null;
            } else {
                targetType = Nullable.GetUnderlyingType(targetType);
                typedValue = Reflections.StringToTypedValue(sourceString, targetType, null);
            }
            return typedValue;
        }

        public static T StringToTypedValue<T>(string sourceString, CultureInfo culture = null) {
            return (T)Reflections.StringToTypedValue(sourceString, typeof(T), culture);
        }

        public static string TypedValueToString(object rawValue, CultureInfo culture = null, string unsupportedReturn = null) {
            if (rawValue == null) {
                return string.Empty;
            }
            if (culture == null) {
                culture = CultureInfo.CurrentCulture;
            }
            Type type = rawValue.GetType();
            string str = null;
            if (type == typeof(string)) {
                str = rawValue as string;
            } else if (type == typeof(int) || type == typeof(decimal) || type == typeof(double) || type == typeof(float) || type == typeof(float)) {
                str = string.Format(culture.NumberFormat, "{0}", rawValue);
            } else if (type == typeof(DateTime)) {
                str = string.Format(culture.DateTimeFormat, "{0}", rawValue);
            } else if (type == typeof(bool) || type == typeof(byte) || type.IsEnum) {
                str = rawValue.ToString();
            } else if (type != typeof(Guid?)) {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter == null || !converter.CanConvertTo(typeof(string)) || !converter.CanConvertFrom(typeof(string))) {
                    str = (string.IsNullOrEmpty(unsupportedReturn) ? rawValue.ToString() : unsupportedReturn);
                } else {
                    str = converter.ConvertToString(null, culture, rawValue);
                }
            } else {
                if (rawValue != null) {
                    return rawValue.ToString();
                }
                str = string.Empty;
            }
            return str;
        }
    }
}