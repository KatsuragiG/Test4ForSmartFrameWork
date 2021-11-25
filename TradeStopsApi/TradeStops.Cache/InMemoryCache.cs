using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;
using TradeStops.Common.Utils;

// consider to use Microsoft.Extensions.Caching.Memory over System.Runtime.Caching (not sure about benefits)

namespace TradeStops.Cache
{
    public sealed class InMemoryCache : IMemoryCache, ICache, IDisposable
    {
        private readonly MemoryCache _memoryCache = new MemoryCache(Guid.NewGuid().ToString());

        private long _hits = 0;
        private long _misses = 0;

        public string CacheKeyPrefix => string.Empty;

        public T Get<T>(string key)
        {
            var genericResult = GetGeneric<T>(key);
            return genericResult == null ? default(T) : genericResult.CacheObject;
        }

        public CacheObjectWrapper<T> GetGeneric<T>(string key)
        {
            var value = _memoryCache.Get(key);

            var result = ToGenericCacheObject<T>(key, value);

            if (result == null)
            {
                Interlocked.Increment(ref _misses);
            }
            else
            {
                Interlocked.Increment(ref _hits);
            }

            return result;
        }

        public List<CacheObjectWrapper<T>> GetMultipleGeneric<T>(IEnumerable<string> keys)
        {
            var results = new List<CacheObjectWrapper<T>>();
            foreach (var key in keys)
            {
                var result = GetGeneric<T>(key);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public void Set<T>(string key, T value, double secondsToExpire)
        {
            var cacheValue = ToCacheValue(value);

            _memoryCache.Set(key, cacheValue, DateTime.UtcNow.AddSeconds(secondsToExpire));
        }

        public void SetMultiple<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, double secondsToExpire)
        {
            foreach (var pair in keyValuePairs)
            {
                Set(pair.Key, pair.Value, secondsToExpire);
            }
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public CacheStats GetCacheStats()
        {
            return new CacheStats
            {
                Hits = _hits,
                Misses = _misses,
                NumberOfItems = _memoryCache.GetCount()
            };
        }

        public void Dispose()
        {
            _memoryCache?.Dispose();
        }

        private CacheObjectWrapper<T> ToGenericCacheObject<T>(string key, object value)
        {
            if (value == null)
            {
                // there is no object in cache
                return null;
            }
            else
            {
                if (value is T)
                {
                    // there's an object exists for required key with required type
                    return new CacheObjectWrapper<T>(key, (T)value);
                }
                else
                {
                    // cached negative case: there's an empty object in cache (new object() without any data)
                    return new CacheObjectWrapper<T>(key, default(T));
                }
            }
        }

        private object ToCacheValue<T>(T value)
        {
            if (ObjectUtils.IsEqualsDefaultValue(value))
            {
                return new object();
            }
            else
            {
                return value;
            }
        }
    }
}
