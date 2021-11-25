using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Grid parameters for search.
    /// </summary>
    // obsolete: use corresponding enum values for indicating fields to search and sort. See SearchUserContract as example
    public class GridSearchContract
    {
        /// <summary>
        /// Value to search for
        /// </summary>
        public string SearchFor { get; set; }

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
        public string OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
