using TradeStops.Cache;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public class ChartsClientCache : BaseCache, IChartsClientCache
    {
        private readonly IClientCacheSettings _clientCacheSettings;

        private readonly IChartsClient _chartsClient;

        public ChartsClientCache(
            ICache cache,
            IClientCacheSettings clientCacheSettings,
            IChartsClient chartsClient)
            : base(cache, clientCacheSettings.TempItemsExpirationInSeconds)
        {
            _clientCacheSettings = clientCacheSettings;
            _chartsClient = chartsClient;
        }

        public ValueChartDataContract GetPureQuantIndicatorChartItems(GetPureQuantIndicatorChartItemsContract chartItemsData)
        {
            var cacheKey = $"{chartItemsData.MarketOutlookId}_" +
                           $"{chartItemsData.Threshold}_" +
                           $"{chartItemsData.FromDate}_" +
                           $"{chartItemsData.ToDate}" +
                           $"_{string.Join("_", chartItemsData.ChartItemsToLoad)}";

            return Get(() =>
                    _chartsClient.GetPureQuantIndicatorChartItems(chartItemsData),
                    cacheKey,
                    _clientCacheSettings.PureQuantIndicatorChartDataExpirationInSeconds);
        }
    }
}
