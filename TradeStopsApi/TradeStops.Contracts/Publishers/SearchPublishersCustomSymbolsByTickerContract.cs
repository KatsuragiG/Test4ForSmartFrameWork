namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameter to find custom symbols
    /// </summary>
    public class SearchPublishersCustomSymbolsByTickerContract
    {
        /// <summary>
        /// Symbol Ticker or part of Ticker.
        /// </summary>
        public string SymbolTicker { get; set; }
    }
}
