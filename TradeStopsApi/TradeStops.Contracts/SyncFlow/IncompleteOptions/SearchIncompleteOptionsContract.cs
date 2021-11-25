using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Filters for incomplete options grid.
    /// </summary>
    public class SearchIncompleteOptionsContract
    {
        /// <summary>
        ///  Synchronization vendor type filter.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  Incomplete options range from that date in UTC.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///  Incomplete options range to that date in UTC.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        ///  (optional) Incomplete option status filter.
        /// </summary>
        public IncompleteOptionStatusTypes? IncompleteOptionStatusType { get; set; }

        /// <summary>
        ///  (optional) Financial institution name filter.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///  Search key word.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        ///  Search by field.
        /// </summary>
        public SearchIncompleteOptionsSearchByFields? SearchByField { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public SearchIncompleteOptionsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
