using System;
using System.Collections.Generic;
using System.Linq;
using TradeStops.Common.Utils;

namespace TradeStops.Common.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Add items to list only if condition is true
        /// </summary>
        /// <param name="list">List for adding items</param>
        /// <param name="condition">Boolean value to determine if items should be added to list or not</param>
        /// <param name="itemsToAdd">Items to add to to the list, cannot be null</param>
        /// <typeparam name="T">The type of items to add.</typeparam>
        public static void AddRangeByCondition<T>(this List<T> list, bool condition, IEnumerable<T> itemsToAdd)
        {
            if (condition)
            {
                list.AddRange(itemsToAdd);
            }
        }

        /// <summary>
        /// Add item to list only if condition is true
        /// </summary>
        /// <param name="list">List for adding items</param>
        /// <param name="condition">Boolean value to determine if item should be added to list or not</param>
        /// <param name="itemToAdd">Item to add to to the list, cannot be null</param>
        /// <typeparam name="T">The type of item to add.</typeparam>
        public static void AddItemByCondition<T>(this List<T> list, bool condition, T itemToAdd)
        {
            if (condition)
            {
                list.Add(itemToAdd);
            }
        }

        public static void AddIfNotDefault<T>(this List<T> list, T itemToAdd)
        {
            if (!ObjectUtils.IsEqualsDefaultValue(itemToAdd))
            {
                list.Add(itemToAdd);
            }
        }

        ////public static List<T> With<T>(this List<T> list, params T[] items)
        ////{
        ////    var result = new List<T>(list);
        ////    result.AddRange(items);

        ////    return result;
        ////}

        /// <summary>
        /// Check if the current list contains any item from the second list.
        /// It is basically the following: list.Contains(items[0]) || list.Contains(items[1]) || ...
        /// If you can compare items without comparer, then it should be enough to use just list.Intersect(otherList).Any()
        /// </summary>
        /// <typeparam name="T">Type of the list item</typeparam>
        /// <param name="list">Current list</param>
        /// <param name="items">Another list</param>
        /// <param name="comparer">Comparer for items</param>
        /// <returns>Returns true in case if there's at least one common item in both lists.</returns>
        public static bool ContainsAny<T>(this List<T> list, IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            return items.Any(x => list.Contains(x, comparer));
        }
    }
}
