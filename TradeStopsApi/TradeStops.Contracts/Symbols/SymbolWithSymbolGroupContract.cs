namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol group
    /// </summary>
    public class SymbolWithSymbolGroupContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol Group
        /// </summary>
        public SymbolGroupContract SymbolGroup { get; set; }
    }
}
