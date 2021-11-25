using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeStops.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Group List to Dictionary with Lists by specified key.
        /// There's also useful DictionaryExtensions.GetListNotNull method to be used in pair with this method.
        /// Consider to use System.Linq.Enumerable.ToLookup extension method instead,
        /// as built-in methods are usually better choice than custom written methods.
        /// </summary>
        /// <param name="list">list of items, for example 'List of PositionItems'</param>
        /// <param name="keySelector">key to group, for example 'x => x.PortoflioId'</param>
        /// <returns>dictionary with grouped lists as values, for example 'result[portfolioId] = List of PositionItems with this portfolioId'</returns>
        public static Dictionary<TKey, List<TElement>> GroupToDictionary<TKey, TElement>(this IEnumerable<TElement> list, Func<TElement, TKey> keySelector)
        {
            var result = list.GroupBy(keySelector).ToDictionary(g => g.Key, g => g.ToList());

            return result;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        // unnecessary in net472+ and netcore2+
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Obtains the data as a list; if it is *already* a list, the original object is returned without
        /// any duplication; otherwise, ToList() is invoked.
        /// Code is taken from Dapper assembly from AsList method.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        /// <param name="source">The enumerable to return as a list.</param>
        /// <returns> The data as a list </returns>
        public static List<T> ToListOrCast<T>(this IEnumerable<T> source)
        {
            return (source == null || source is List<T>) ? (List<T>)source : source.ToList();
        }    
    }
}
