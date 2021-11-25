using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for displaying searches that are located in the common table.
    /// The contract is called `common` because each user has the right to get a search from the common table.
    /// </summary>
    public class StockFinderSearchCommonContract
    {
        /// <summary>
        /// ID of the search
        /// </summary>
        public int StockFinderSearchId { get; set; }

        /// <summary>
        /// Screener search type
        /// </summary>
        public ScreenerTypes SearchType { get; set; }

        /// <summary>
        /// The values of search
        /// </summary>
        public List<StockFinderSearchFilterContract> SearchFilters { get; set; }

        /// <summary>
        /// Default name
        /// </summary>
        public string DefaultName { get; set; }

        /// <summary>
        /// Number of displayed values
        /// </summary>
        public int? DisplayCount { get; set; }

        /// <summary>
        /// Order by field
        /// </summary>
        public string OrderByField { get; set; }

        /// <summary>
        /// Order direction
        /// </summary>
        public OrderTypes? OrderType { get; set; }
    }
}
