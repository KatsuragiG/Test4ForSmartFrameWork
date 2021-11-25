namespace TradeStops.Contracts
{
    /// <summary>
    /// The value allocation group by the symbol
    /// </summary>
    public class ValueAllocationSymbolGroupContract
    {
        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol name (AAPL)
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Company name (Apple)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of all positions with current symbol
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// The percentage value of all positions with current symbol. Depend on the total portfolios value.
        /// </summary>
        public decimal ValuePercentage { get; set; }
    }
}
