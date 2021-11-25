using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Most Popular Ticker
    /// </summary>
    public class MostPopularTickerContract
    {
        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest price
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Current VQ value
        /// </summary>
        public decimal CurrentVq { get; set; }

        /// <summary>
        /// Daily gain for symbol
        /// </summary>
        public decimal DailyGain { get; set; }

        /// <summary>
        /// Daily gain percentage
        /// </summary>
        public decimal DailyGainPercentage { get; set; }

        /// <summary>
        /// Current SSI state
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }
    }
}
