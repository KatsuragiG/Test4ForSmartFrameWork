using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Stock Finder Search
    /// </summary>
    public class CreateStockFinderSearchContract
    {
        /// <summary>
        /// The name of search
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the screener is individual for each user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Screener search type
        /// </summary>
        public ScreenerTypes? SearchType { get; set; }

        /// <summary>
        /// The items of search
        /// </summary>
        public List<StockFinderSearchFilterContract> SearchFilters { get; set; }

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
