namespace TradeStops.Contracts
{
    /// <summary>
    /// Currency
    /// </summary>
    public class CurrencyContract
    {
        /// <summary>
        /// Currency ID.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Currency name, like 'USD'.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Currency symbol, like '$'.
        /// </summary>
        public string Symbol { get; set; }
    }
}
