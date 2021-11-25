using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    ///  Filters for sync report portfolios grid.
    /// </summary>
    public class SearchSyncReportsPortfoliosStatisticsContract
    {
        /// <summary>
        /// Vendor sync log ID.
        /// </summary>
        public int VendorSyncLogId { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public SearchSyncReportsPortfoliosStatisticsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
