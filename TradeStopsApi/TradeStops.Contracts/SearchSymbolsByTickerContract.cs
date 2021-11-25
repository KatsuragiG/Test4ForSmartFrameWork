namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to find all symbols (stocks and options)
    /// </summary>
    public class SearchSymbolsByTickerContract
    {
        /// <summary>
        /// Symbol Ticker or part of Ticker.
        /// </summary>
        public string SymbolTicker { get; set; }

        /// <summary>
        /// Property to identify whether the deleted options are included/not included/included all on the list.
        /// </summary>
        public bool? Delisted { get; set; }

        /// <summary>
        /// Property to identify whether it’s necessary to return only options/without options or all symbols.
        /// </summary>
        public bool? Option { get; set; }

        /// <summary>
        /// Property to identify whether it’s necessary to return only symbols with exact match by symbol ticker.
        /// </summary>
        public bool IsExactMatch { get; set; }

        /// <summary>
        /// Maximal number of symbols to return. If not specified the default value = 9999 will be used.
        /// </summary>
        public int? MaxResults { get; set; }
    }
}