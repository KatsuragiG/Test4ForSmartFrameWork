namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about exchange market
    /// </summary>
    public class ExchangeMarketContract
    {
        /// <summary>
        /// Exchange ID.
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Friendly name of exchange
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Short name of exchange
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Country ID of exchange
        /// </summary>
        public int ExchangeCountryId { get; set; }
    }
}
