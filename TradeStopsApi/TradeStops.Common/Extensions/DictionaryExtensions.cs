using System;
using System.Collections.Generic;

namespace TradeStops.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Get list from dictionary, if it have no value for specified key then empty list will be returned
        /// </summary>
        /// <param name="dictionary">dictionary created by GroupToDictionary extension method</param>
        /// <param name="key">key for this dictionary</param>
        /// <returns>list for specified key, or empty list if dictionary doesn't contain this key</returns>
        ////[Obsolete("Use 'Lookup' type instead of Dictionary in such cases. If you try to get values by missing ID it returns emtpy enumeration by default.")]
        public static List<TElement> GetListNotNull<TKey, TElement>(this Dictionary<TKey, List<TElement>> dictionary, TKey key)
        {
            var result = dictionary.ContainsKey(key) ? dictionary[key] : new List<TElement>();

            return result;
        }

        public static TElement GetElementOrNull<TKey1, TKey2, TElement>(this Dictionary<TKey1, Dictionary<TKey2, TElement>> dictionary, TKey1 key1, TKey2 key2)
            where TElement : class
        {
            var innerDictionary = dictionary.GetElementOrNull(key1);

            return innerDictionary?.GetElementOrNull(key2);
        }

        public static TElement? GetValueOrNull<TKey, TElement>(this Dictionary<TKey, TElement> dictionary, TKey key)
            where TElement : struct
        {
            TElement value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static TElement GetElementOrNull<TKey, TElement>(this Dictionary<TKey, TElement> dictionary, TKey key) 
            where TElement : class
        {
            // FIY:
            // MyClass myVar = null;
            // myVar.MyExtensionMethod() <- it doesn't throw null reference exception like instance method does at this moment
            // because call to extension method converted to MyExtensions.MyExtensionMethod(myVar)

            TElement value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static TElement GetElementOrNull<TKey, TElement>(this Dictionary<TKey, TElement> dictionary, TKey? key)
            where TElement : class
            where TKey : struct
        {
            if (key == null)
            {
                return null;
            }

            return GetElementOrNull(dictionary, key.Value);
        }
    }
}
