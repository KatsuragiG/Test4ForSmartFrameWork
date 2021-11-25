using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeStops.Cache
{
    /// <summary>
    /// This class-wrapper is used because we want to cache negative cases when item was not found in repository,
    /// but we can't just cache null value by default.
    /// If object of this class equals null, it means that there are no such object in cache at all.
    /// If CacheObject equals default(T), it means that we cached negative case.
    /// </summary>
    public class CacheObjectWrapper<T>
    {
        public CacheObjectWrapper(string cacheKey, T cacheObject)
        {
            CacheKey = cacheKey;
            CacheObject = cacheObject;
        }

        public CacheObjectWrapper()
        {
        }

        public string CacheKey { get; set; }

        public T CacheObject { get; set; }
    }
}
