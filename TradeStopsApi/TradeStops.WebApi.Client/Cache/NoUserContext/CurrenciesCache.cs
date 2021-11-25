using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Cache;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public class CurrenciesClientCache : BaseCache, ICurrenciesClientCache
    {
        private readonly IClientCacheSettings _clientCacheSettings;

        private readonly ICurrenciesClient _currenciesClient;

        public CurrenciesClientCache(
            ICache cache,
            IClientCacheSettings clientCacheSettings,
            ICurrenciesClient currenciesClient)
            : base(cache, clientCacheSettings.TempItemsExpirationInSeconds)
        {
            _clientCacheSettings = clientCacheSettings;
            _currenciesClient = currenciesClient;
        }

        public List<CurrencyContract> GetAll()
        {
            return Get(() => _currenciesClient.GetAll(), string.Empty, _clientCacheSettings.CurrenciesExpirationInSeconds);
        }

        public async Task<List<CurrencyContract>> GetAllAsync()
        {
            return await Get(async () => await _currenciesClient.GetAllAsync(), string.Empty, _clientCacheSettings.CurrenciesExpirationInSeconds);
        }

        public Dictionary<int, Dictionary<int, decimal>> GetAllCrossCoursesNotNull()
        {
            return Get(() => _currenciesClient.GetAllCrossCoursesNotNull(), string.Empty, _clientCacheSettings.CrossCoursesExpirationInSeconds);
        }

        public async Task<Dictionary<int, Dictionary<int, decimal>>> GetAllCrossCoursesNotNullAsync()
        {
            return await Get(async () => await _currenciesClient.GetAllCrossCoursesNotNullAsync(), string.Empty, _clientCacheSettings.CrossCoursesExpirationInSeconds);
        }
    }
}
