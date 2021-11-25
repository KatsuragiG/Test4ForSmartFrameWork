using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Cache;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public class FinancialInstitutionsClientCache : BaseCache, IFinancialInstitutionsClientCache
    {
        private readonly IClientCacheSettings _clientCacheSettings;

        private readonly IAdminFinancialInstitutionsClient _adminFinancialInstitutionsClient;

        public FinancialInstitutionsClientCache(
            ICache cache,
            IClientCacheSettings clientCacheSettings,
            IAdminFinancialInstitutionsClient adminFinancialInstitutionsClient)
            : base(cache, clientCacheSettings.TempItemsExpirationInSeconds)
        {
            _clientCacheSettings = clientCacheSettings;
            _adminFinancialInstitutionsClient = adminFinancialInstitutionsClient;
        }

        public List<AdminFinancialInstitutionContract> GetAllFinancialInstitutionsWithBetaTesters()
        {
            return Get(() => 
                _adminFinancialInstitutionsClient.GetAllFinancialInstitutionsWithBetaTesters(), 
                string.Empty, 
                _clientCacheSettings.FinancialInstitutionsExpirationInSeconds);
        }

        public Task<List<AdminFinancialInstitutionContract>> GetAllFinancialInstitutionsWithBetaTestersAsync()
        {
            return Get(async () => 
                await _adminFinancialInstitutionsClient.GetAllFinancialInstitutionsWithBetaTestersAsync(), 
                string.Empty, 
                _clientCacheSettings.FinancialInstitutionsExpirationInSeconds);
        }
    }
}
