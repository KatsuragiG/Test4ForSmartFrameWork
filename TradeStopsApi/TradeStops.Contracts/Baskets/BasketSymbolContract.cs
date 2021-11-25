using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Basket Symbol
    /// </summary>
    public class BasketSymbolContract
    {
        /// <summary>
        /// Basket Symbol ID
        /// </summary>
        public int BasketSymbolId { get; set; }

        /// <summary>
        /// Basket ID
        /// </summary>
        public int BasketId { get; set; }

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
        /// Daily gain for symbol
        /// </summary>
        public decimal DailyGain { get; set; }

        /// <summary>
        /// Daily gain percentage
        /// </summary>
        public decimal DailyGainPercentage { get; set; }

        /// <summary>
        /// Short percent of float
        /// </summary>
        public decimal? ShortPercentOfFloat { get; set; }
    }
}
