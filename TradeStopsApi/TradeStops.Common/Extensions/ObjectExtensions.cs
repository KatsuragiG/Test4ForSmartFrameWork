using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TradeStops.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Use this method to pass single item to bulk methods
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An List consisting of a single item. </returns>
        public static List<T> AsList<T>(this T item)
        {
            return new List<T> { item };
        }

        //public static IEnumerable<T> Yield<T>(this T item)
        //{
        //    yield return item;
        //}

        /// <summary>
        /// Usage: if (longVariableName.In(1, 6, 9, 11)) { ... }.
        /// It's ok to call this extension method for null variable
        /// </summary>
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        //internal static Wrapper<T> Wrap<T>(this T obj)
        //{
        //    return new Wrapper<T>(obj);
        //}
    }
}
