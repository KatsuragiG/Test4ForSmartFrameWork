namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about symbol
    /// </summary>
    public class SymbolContract
    {
        /// <summary>
        /// General information about symbol that applicable for all types of symbols. Not null for all types of symbols.
        /// </summary>
        public StockContract Stock { get; set; }

        /// <summary>
        /// Option-specific data. Not null only for options; null for other types of symbols
        /// </summary>
        public OptionContract Option { get; set; }
    }
}
