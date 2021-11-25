using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about symbols from news
    /// </summary>
    public class NewsSymbolContract // todo: consider to create separate contract like SymbolLatestPriceStatisticsContract with daily gain + latest/previous price value (consider to add it to PriceContract)
    {
        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency values
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest price values. Can be null for symbols without prices
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Daily gain in symbol's currency. Can be null for symbols without prices
        /// </summary>
        public decimal? DailyGain { get; set; }

        /// <summary>
        /// Daily gain in percents. Can be null for symbols without prices
        /// </summary>
        public decimal? DailyGainPercentage { get; set; }
    }
}
