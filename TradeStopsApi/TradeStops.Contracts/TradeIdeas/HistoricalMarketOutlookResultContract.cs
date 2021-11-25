using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Hisorical Market Outlook values
    /// </summary>
    public class HistoricalMarketOutlookResultContract
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
        /// Close price for specified TradeDate param
        /// </summary>
        public PriceContract TradeClosePrice { get; set; }

        /// <summary>
        /// First price
        /// </summary>
        public PriceContract FirstPrice { get; set; }

        /// <summary>
        /// Min, max and current prices for 1 year
        /// </summary>
        public PriceRangeContract PriceRange { get; set; }

        /// <summary>
        /// VQ value
        /// </summary>
        public decimal VqValue { get; set; }

        /// <summary>
        /// Historical SSI value as of TradeDate
        /// </summary>
        public SsiHistoryValueContract HistoricalSsiValue { get; set; }

        /// <summary>
        /// Percentage distribution of SSI
        /// </summary>
        public SsiDistributionContract SsiDistribution { get; set; }

        /// <summary>
        /// Bull Bear indicator value as of TradeDate
        /// </summary>
        public BullBearIndicatorContract BullBearIndicator { get; set; }

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
    }
}
