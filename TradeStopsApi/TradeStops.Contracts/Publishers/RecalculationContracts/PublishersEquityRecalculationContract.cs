namespace TradeStops.Contracts
{
    /// <summary>
    /// Data for calculation of equity.
    /// </summary>
    public class PublishersEquityRecalculationContract
    {
        /// <summary>
        /// Defines whether a trade is long.
        /// </summary>
        public bool IsLongTrade { get; set; }

        /// <summary>
        /// Open value.
        /// </summary>
        public decimal OpenValue { get; set; }

        /// <summary>
        /// Close value.
        /// </summary>
        public decimal CloseValue { get; set; }

        /// <summary>
        /// Sum of dividends.
        /// </summary>
        public decimal? Dividends { get; set; }
    }
}
