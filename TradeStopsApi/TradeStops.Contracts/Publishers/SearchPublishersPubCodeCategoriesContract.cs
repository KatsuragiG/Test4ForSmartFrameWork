namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search categories of customer
    /// </summary>
    public class SearchPublishersPubCodeCategoriesContract
    {
        /// <summary>
        /// Pub code categories start with query string.
        /// </summary>
        public string Query { get; set; }
    }
}
