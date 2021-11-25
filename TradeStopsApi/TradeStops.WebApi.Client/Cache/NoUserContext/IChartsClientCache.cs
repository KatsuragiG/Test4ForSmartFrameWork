using TradeStops.Contracts;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public interface IChartsClientCache
    {
        ValueChartDataContract GetPureQuantIndicatorChartItems(GetPureQuantIndicatorChartItemsContract chartItemsData);
    }
}
