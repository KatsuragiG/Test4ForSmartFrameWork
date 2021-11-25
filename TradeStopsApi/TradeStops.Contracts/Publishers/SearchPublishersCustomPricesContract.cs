using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search custom price
    /// </summary>
    public class SearchPublishersCustomPricesContract
    {
        /// <summary>
        /// Prices for custom symbol id
        /// </summary>
        public int CustomSymbolId { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Order by field.
        /// </summary>
        public SearchCustomPricesOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}