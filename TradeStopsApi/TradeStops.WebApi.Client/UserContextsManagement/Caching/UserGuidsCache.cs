using System;
using Microsoft.Extensions.Caching.Memory;

namespace TradeStops.WebApi.Client.UserContextsManagement.Caching
{
    internal sealed class UserGuidsCache : IDisposable
    {
        private const string UserGuidKeyPrefix = nameof(UserGuidKeyPrefix);

        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public Guid? Get(int userId)
        {
            var cacheKey = UserGuidKeyPrefix + userId;

            return _cache.Get(cacheKey) as Guid?;
        }

        public void Set(int userId, Guid? guid, double expirationInSeconds)
        {
            var cacheKey = UserGuidKeyPrefix + userId;

            _cache.Set(cacheKey, guid, DateTimeOffset.UtcNow.AddSeconds(expirationInSeconds));
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
