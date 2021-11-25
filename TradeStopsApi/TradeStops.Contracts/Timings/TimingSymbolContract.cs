namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about Symbol for Timing Page (same like Position Card)
    /// </summary>
    public class TimingSymbolContract
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest available price
        /// </summary>
        public PriceContract LatestPrice { get; set; }

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
