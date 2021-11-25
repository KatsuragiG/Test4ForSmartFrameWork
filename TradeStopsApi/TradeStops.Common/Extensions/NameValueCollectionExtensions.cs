using System.Collections.Specialized;
using System.Linq;

namespace TradeStops.Common.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string GetValueOrNull(this NameValueCollection collection, string key)
        {
            if (collection.AllKeys.Contains(key))
            {
                return collection[key];
            }

            return null;
        }
    }
}
