using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Market Outlook result
    /// </summary>
    public class MarketOutlookResultContract
    {
        /// <summary>
        /// The type of market outlook
        /// </summary>
        public MarketOutlookTypes MarketOutlookId { get; set; }

        /// <summary>
        /// Market outlook group Id
        /// </summary>
        public MarketOutlookGroupIds? MarketOutlookGroupId { get; set; }

        /// <summary>
        /// Symbol group type
        /// </summary>
        public SymbolGroupTypes SymbolGroupType { get; set; }

        /// <summary>
        /// The name of the index
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Latest price
        /// </summary>
        [Obsolete("Use LatestClose instead")]
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// First price
        /// </summary>
        public PriceContract FirstPrice { get; set; }

        /// <summary>
        /// Latest close
        /// </summary>
        public PriceContract LatestClose { get; set; }

        /// <summary>
        /// Latest intraday price.
        /// </summary>
        public IntradayPricesAggregatedDataContract LatestIntradayPrice { get; set; }

        /// <summary>
        /// Min, max and current prices for 1 year
        /// </summary>
        public PriceRangeContract PriceRange { get; set; }

        /// <summary>
        /// VQ value
        /// </summary>
        public decimal VqValue { get; set; }

        /// <summary>
        /// Current SSI state
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// Percentage distribution of SSI
        /// </summary>
        public SsiDistributionContract SsiDistribution { get; set; }

        /// <summary>
        /// Sector name
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Daily gain
        /// </summary>
        public decimal? DailyGain { get; set; }

        /// <summary>
        /// Percentage of daily gain
        /// </summary>
        public decimal? DailyGainPercentage { get; set; }

        /// <summary>
        /// WMA for Pure Quant Percent
        /// </summary>
        public decimal? WmaPureQuantPercent { get; set; }

        /// <summary>
        /// Exchange works today or not.
        /// </summary>
        public bool IsExchangeWorking { get; set; }

        /// <summary>
        /// Availability of intraday price.
        /// </summary>
        public bool IsIntradayPriceAvailable { get; set; }
    }
}
