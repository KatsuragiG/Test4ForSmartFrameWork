using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Commodity result
    /// </summary>
    public class CommodityResultContract
    {
        /// <summary>
        /// The unique id of the commodity
        /// </summary>
        public int CommodityId { get; set; }

        /// <summary>
        /// The name of the commodity
        /// </summary>
        public string Commodity { get; set; }

        /// <summary>
        /// The group to which commodity belongs to
        /// </summary>
        public string CommodityGroup { get; set; }

        /// <summary>
        /// The EFT symbol that represents the commodity
        /// </summary>
        public SymbolContract EtfSymbol { get; set; }

        /// <summary>
        /// Currency of the ETF symbol
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest close price of the ETF symbol
        /// </summary>
        [Obsolete("Use LatestClose instead")]
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Latest close price of the ETF symbol
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
        /// Daily gain
        /// </summary>
        public decimal? DailyGain { get; set; }

        /// <summary>
        /// Percentage of daily gain
        /// </summary>
        public decimal? DailyGainPercentage { get; set; }

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
