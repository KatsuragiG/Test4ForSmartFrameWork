using System;
using System.Threading.Tasks;
using TradeStops.Cache;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.Client.Cache.NoUserContext
{
    public class AdminProductFeaturesClientCache : BaseCache, IAdminProductFeaturesClientCache
    {
        private readonly IClientCacheSettings _clientCacheSettings;

        private readonly IAdminProductFeaturesClient _adminProductFeaturesClient;

        public AdminProductFeaturesClientCache(
            ICache cache,
            IClientCacheSettings clientCacheSettings,
            IAdminProductFeaturesClient adminProductFeaturesClient)
            : base(cache, clientCacheSettings.TempItemsExpirationInSeconds)
        {
            _clientCacheSettings = clientCacheSettings;
            _adminProductFeaturesClient = adminProductFeaturesClient;
        }

        public async Task<AdminFeaturesContract> GetAdminFeaturesAsync(int tradeSmithUserId, DateTime lastLoginDate)
        {
            return await GetAsync(
                async () => await _adminProductFeaturesClient.GetAdminFeaturesAsync(tradeSmithUserId),
                $"{tradeSmithUserId}_{lastLoginDate}",
                _clientCacheSettings.UserAdminFeaturesExpirationInSeconds);
        }
    }
}
