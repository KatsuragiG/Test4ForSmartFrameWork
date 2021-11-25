using System;
using Microsoft.Extensions.Caching.Memory;

namespace TradeStops.WebApi.Client.UserContextsManagement.Caching
{
    internal sealed class UserContextsCache : IUserContextsCache, IDisposable
    {
        private const string UserContextKeyPrefix = nameof(UserContextKeyPrefix);

        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public UserContext Get(string key)
        {
            var cacheKey = UserContextKeyPrefix + key;

            return _cache.Get(cacheKey) as UserContext;
        }

        public void Set(string key, UserContext userContext)
        {
            var cacheKey = UserContextKeyPrefix + key;
            var expirationInSeconds = GetExpirationInSeconds(userContext.ExpirationDate);

            _cache.Set(cacheKey, userContext, DateTimeOffset.UtcNow.AddSeconds(expirationInSeconds));
        }

        public void Remove(string key)
        {
            var cacheKey = UserContextKeyPrefix + key;

            _cache.Remove(cacheKey);
        }

        private int GetExpirationInSeconds(DateTime expirationDate)
        {
            var expirationInSeconds = (expirationDate - DateTime.Now).TotalSeconds;

            if (expirationInSeconds < 0)
            {
                expirationInSeconds = 0;
            }

            return (int)expirationInSeconds;
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
