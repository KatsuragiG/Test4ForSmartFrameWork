using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Stock Finder Search
    /// </summary>
    public class StockFinderSearchContract
    {
        /// <summary>
        /// ID of the search
        /// </summary>
        public int StockFinderSearchId { get; set; }

        /// <summary>
        /// The name of search
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the screener is individual for each user.
        /// </summary>
        public string Description { get; set; }

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

        /// <summary>
        /// Date of the creation
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Defines whether Stock Finder Search is predefined
        /// </summary>
        public bool IsPredefined { get; set; }
    }
}
