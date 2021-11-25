using System.Collections.Generic;
using System.Linq;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// class provides object comparison methods
    /// </summary>
    public class BaseObjectComparator
    {
        protected BaseObjectComparator()
        {
        }

        /// <summary>
        /// Checking equality of two List
        /// </summary>
        /// <param name="list1">first list</param>
        /// <param name="list2">second list</param>
        /// <typeparam name="T">object type</typeparam>
        /// <returns>comparison result</returns>
        public static bool AreListsEquals<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            return !list1.Where((t, index) => !EqualityComparer<T>.Default.Equals(t, list2[index])).Any();
        }

        /// <summary>
        /// Checking equality of two Dictionary
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool AreDictionariesEquals<TKey, TValue>(Dictionary<TKey, TValue> actual, Dictionary<TKey, TValue> expected)
        {
            if (actual.Count != expected.Count || actual.Keys.Except(expected.Keys).Any() || expected.Keys.Except(actual.Keys).Any())
            {
                return false;
            }

            foreach (var pair in actual)
            {
                if (!EqualityComparer<TValue>.Default.Equals(pair.Value, expected[pair.Key]))
                {
                    return false;
                }
            }
                
            return true;
        }

        /// <summary>
        /// Checking that first list contains second list
        /// </summary>
        /// <param name="list1">first list</param>
        /// <param name="list2">second list</param>
        /// <returns>comparison result</returns>
        public static bool AreListsContains(List<string> list1, List<string> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            var list3 = list2.Select(el => el.Trim()).ToList();
            return list1.All(el => list3.Contains(el.Trim()));
        }

        /// <summary>
        /// Checking that list is fully included into another list
        /// </summary>
        /// <param name="smallList"></param>
        /// <param name="anotherList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>comparison result</returns>
        public static bool IsListIsFullyIncludedIntoAnother<T>(List<T> smallList, List<T> anotherList)
        {
            var result = true;
            var secondListQuantity = anotherList.Count;
            for (int i = 0; i < smallList.Count; i++)
            {
                var quanitytOfMithmatches = 0;
                for (int j = 0; j < secondListQuantity; j++)
                {
                    if (!smallList[i].Equals(anotherList[i]))
                    {
                        quanitytOfMithmatches++;
                    }
                }
                if (quanitytOfMithmatches == secondListQuantity)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Find elements that are not represented in the second file
        /// </summary>
        /// <param name="list1">first list</param>
        /// <param name="list2">second list</param>
        /// <typeparam name="T">object type</typeparam>
        /// <returns>elements that are not represented in the second file</returns>
        public static List<T> FindItemsNotPresentIn2Lists<T>(List<T> list1, List<T> list2)
        {
            var list = list1.Count > list2.Count ? list1 : list2;
            return list.Where(t => !list1.Contains(t) && !list2.Contains(t)).ToList();
        }
    }
}