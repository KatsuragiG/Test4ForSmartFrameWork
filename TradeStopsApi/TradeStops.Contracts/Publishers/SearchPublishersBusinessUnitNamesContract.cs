namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search business unit names
    /// </summary>
    public class SearchPublishersBusinessUnitNamesContract
    {
        /// <summary>
        /// Business unit names start with query string.
        /// </summary>
        public string Query { get; set; }
    }
}
