using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search statistics by brokers for sync reports.
    /// </summary>
    public class SearchSyncReportsBrokersStatisticsContract
    {
        /// <summary>
        ///  Synchronization vendor type filter.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  Brokers statistics range from that date in UTC.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///  Brokers statistics range to that date in UTC.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        ///  (optional) Sync service type.
        /// </summary>
        public SyncServiceTypes? SyncServiceType { get; set; }

        /// <summary>
        /// (optional) Field to use for search value.
        /// </summary>
        public SearchSyncReportsBrokersStatisticsFields? SearchByField { get; set; }

        /// <summary>
        /// (optional) Value to search for.
        /// </summary>
        public string SearchValue { get; set; }

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
        public SearchSyncReportsBrokersStatisticsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
