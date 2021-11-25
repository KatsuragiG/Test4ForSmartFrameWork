using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace TradeStops.Common.Utils
{
    public static class ObjectUtils
    {
        public static bool IsEqualsDefaultValue<T>(T item)
        {
            // works without boxing http://stackoverflow.com/questions/65351/null-or-default-comparison-of-generic-argument-in-c-sharp
            return EqualityComparer<T>.Default.Equals(item, default(T));
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            return GetPropertyInfo(propertyLambda).Name;
        }

        /// <summary>
        /// Get PropertyInfo with attributes of a property from any class.
        /// Works only for properties, throws exception for methods and fields.
        /// Usage: GetPropertyInfo((SomeType x) => x.Property);
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="TProperty">Property type</typeparam>
        /// <param name="propertyLambda">Property expression</param>
        /// <returns>Name of the property</returns>
        // https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        private static PropertyInfo GetPropertyInfo<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            var type = typeof(T);

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }
    }
}
