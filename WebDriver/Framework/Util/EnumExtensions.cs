using System;
using System.Collections.Generic;
using System.Linq;

namespace WebdriverFramework.Framework.Util
{

    /// <summary>
    /// getting additional attrubutes from Enum
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// getting string mapping from Enum
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetStringMapping(this Enum obj)
        {
            return GetAttribute<StringMappingAttribute>(obj).StringName;
        }

        /// <summary>
        /// getting Description attribute from Enum
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            return GetAttribute<DescriptionAttribute>(obj).StringName;
        }

        /// <summary>
        /// getting value from Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetValue(this Enum value)
        {
            return (int)Enum.ToObject(value.GetType(), value);
        }

        /// <summary>
        /// getting attribute from Enum
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }

        /// <summary>
        /// string mapping attribute from Enum
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class StringMappingAttribute : Attribute
        {
            /// <summary>
            /// String name for String Mapping Attribute
            /// </summary>
            public string StringName { get; }

            /// <summary>
            /// String names for String Mapping Attribute
            /// </summary>
            public string[] StringNames { get; }

            /// <summary>
            /// String Mapping Attribute to keep meta data on Enum lelvel
            /// </summary>
            /// <param name="stringName"></param>
            public StringMappingAttribute(string stringName)
            {
                this.StringName = stringName;
            }
            /// <summary>
            /// String Mapping Attribute to keep meta data on Enum lelvel
            /// </summary>
            /// <param name="stringNames"></param>
            public StringMappingAttribute(string[] stringNames)
            {
                this.StringNames = stringNames;
            }
        }
        /// <summary>
        /// Enum extesions Utils 
        /// </summary>
        public static class EnumUtility
        {
            /// <summary>
            /// get values  of attribute
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IEnumerable<T> GetValues<T>()
            {
                return Enum.GetValues(typeof(T)).Cast<T>();
            }
            /// <summary>
            /// get value of attribute
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="skipCount"></param>
            /// <returns></returns>
            public static IEnumerable<T> GetValues<T>(int skipCount)
            {
                return GetValues<T>().Skip(skipCount);
            }
            /// <summary>
            /// get name of attribute
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string GetName<T>(int value)
            {
                return Enum.GetName(typeof(T), value);
            }
        }
        /// <summary>
        /// Is Object Has Simple Type to get model comparison for object and their collection
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsObjectHasSimpleType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetGenericArguments()[0].IsObjectHasSimpleType();
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || IsObjectDoubleDecimalIntType(type);
        }
        private static bool IsObjectDoubleDecimalIntType(this Type type)
        {
            return type == typeof(double)
                   || type == typeof(int)
                   || type == typeof(decimal);
        }
        /// <summary>
        /// Description Attribute to keep meta data on Enum level
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]        
        public class DescriptionAttribute : Attribute
        {
            /// <summary>
            /// String name for Description Attribute
            /// </summary>
            public string StringName { get; }

            /// <summary>
            /// String names for Description Attribute
            /// </summary>
            public string[] StringNames { get; }

            /// <summary>
            /// get value of Description Attribute
            /// </summary>
            /// <param name="stringName"></param>
            public DescriptionAttribute(string stringName)
            {
                this.StringName = stringName;
            }
            /// <summary>
            /// get values of Description Attribute
            /// </summary>
            /// <param name="stringNames"></param>
            public DescriptionAttribute(string[] stringNames)
            {
                this.StringNames = stringNames;
            }
        }
    }
}