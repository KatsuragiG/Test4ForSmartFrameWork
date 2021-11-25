namespace TradeStops.Contracts
{
    /// <summary>
    /// Current SSI value
    /// </summary>
    public class SsiCurrentValueContract
    {
        /// <summary>
        /// ID of the requested symbol.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// SSI values for long positions.
        /// </summary>
        public SsiValueContract Long { get; set; }

        /// <summary>
        /// SSI values for long positions adjusted by dividends.
        /// </summary>
        public SsiValueContract LongAdj { get; set; }

        /// <summary>
        /// SSI values for short positions.
        /// </summary>
        public SsiValueContract Short { get; set; }        
    }
}
