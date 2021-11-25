using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Cache;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public class VendorSyncErrorsClientCache : BaseCache, IVendorSyncErrorsClientCache
    {
        private readonly IClientCacheSettings _clientCacheSettings;

        private readonly IVendorSyncErrorsClient _vendorSyncErrorsClient;

        public VendorSyncErrorsClientCache(ICache cache, IClientCacheSettings clientCacheSettings, IVendorSyncErrorsClient vendorSyncErrorsClient)
            : base(cache, clientCacheSettings.TempItemsExpirationInSeconds)
        {
            _clientCacheSettings = clientCacheSettings;
            _vendorSyncErrorsClient = vendorSyncErrorsClient;
        }

        public async Task<List<VendorSyncErrorMessageContract>> GetAllAsync()
        {
            return await Get(async () => await _vendorSyncErrorsClient.GetAllErrorMessagesAsync(), string.Empty, _clientCacheSettings.VendorSyncErrorsExpirationInSeconds);
        }

        public List<VendorSyncErrorMessageContract> GetAll()
        {
            return Get(() => _vendorSyncErrorsClient.GetAllErrorMessages(), string.Empty, _clientCacheSettings.VendorSyncErrorsExpirationInSeconds);
        }
    }
}
