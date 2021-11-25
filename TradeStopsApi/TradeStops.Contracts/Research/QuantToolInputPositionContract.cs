namespace TradeStops.Contracts
{
    /// <summary>
    /// Position for Quant Tool
    /// </summary>
    public class QuantToolInputPositionContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Determines if position should be included to Quant Tool algorithm for rebalance.
        /// </summary>
        public bool IsForRebalance { get; set; }        
    }
}
