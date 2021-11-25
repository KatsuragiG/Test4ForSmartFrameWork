using System.Collections.Generic;

namespace TradeStops.Cache
{
    // todo now: rename to ICacheStorage, same for implementations
    public interface ICache
    {
        /// <summary>
        /// This property is required because we need to separate redis cache items for DEV1 and QA_AUTO.
        /// It can be empty, if there are no any reasons to use it
        /// </summary>
        string CacheKeyPrefix { get; }

        T Get<T>(string key);

        CacheObjectWrapper<T> GetGeneric<T>(string key);

        List<CacheObjectWrapper<T>> GetMultipleGeneric<T>(IEnumerable<string> keys);

        void Set<T>(string key, T value, double secondsToExpire);

        ////void SetGeneric<T>(string key, T value, double secondsToExpire);

        void SetMultiple<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, double secondsToExpire);

        void Remove(string key);

        CacheStats GetCacheStats();
    }
}
