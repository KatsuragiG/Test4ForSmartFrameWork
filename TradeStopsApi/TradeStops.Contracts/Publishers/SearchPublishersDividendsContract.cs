using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search dividends
    /// </summary>
    public class SearchPublishersDividendsContract
    {
        /// <summary>
        /// Subtrade id.
        /// </summary>
        public int SubtradeId { get; set; }

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
        public SearchDividendsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
