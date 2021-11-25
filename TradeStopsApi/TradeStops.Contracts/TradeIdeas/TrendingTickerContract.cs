namespace TradeStops.Contracts
{
    /// <summary>
    /// Trending ticker
    /// </summary>
    public class TrendingTickerContract
    {
        /// <summary>
        /// Symbol contract for corresponding ticker
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency contract for symbol
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Contract with latest price
        /// </summary>
        public PriceContract LatestPrice { get; set; }

        /// <summary>
        /// Contract with previous price
        /// </summary>
        public PriceContract PreviousPrice { get; set; }

        /// <summary>
        /// Volatility Quotient
        /// </summary>
        public decimal VqValue { get; set; }

        /// <summary>
        /// Daily gain
        /// </summary>
        public decimal DailyGain { get; set; }

        /// <summary>
        /// Percentage of daily gain
        /// </summary>
        public decimal DailyGainPercentage { get; set; }
    }
}
