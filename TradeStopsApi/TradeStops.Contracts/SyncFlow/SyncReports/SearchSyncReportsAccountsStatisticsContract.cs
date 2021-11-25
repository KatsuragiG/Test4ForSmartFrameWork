using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    ///  Filters for sync report accounts grid.
    /// </summary>
    public class SearchSyncReportsAccountsStatisticsContract
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
        ///  (optional) If true then returns only successful logs, if false - only failed.
        /// </summary>
        public bool? IsFilteredBySuccessLogs { get; set; }

        /// <summary>
        ///  (optional) Financial institution name filter.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  (optional) Vendor error code to filter.
        /// </summary>
        public YodleeErrorCodeTypes? ErrorCode { get; set; }

        /// <summary>
        ///  (optional) Vendor error name o filter.
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        ///  (optional) If true then returns only generic errors.
        /// </summary>
        public bool? IsFilteredByGenericError { get; set; }

        /// <summary>
        ///  (optional) Vendor sync service type filter.
        /// </summary>
        public SyncServiceTypes? SyncServiceType { get; set; }

        /// <summary>
        ///  (optional) Vendor sync process type filter.
        /// </summary>
        public SyncProcessType? SyncProcessType { get; set; }

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
        ///  Search by field:
        ///    VendorAccountId - Exact match mode
        ///    UserEmail - Start match mode
        ///    VendorUsername - Exact match mode
        ///    FinancialInstitutionName - Anywhere match mode
        ///    ErrorDescription - Anywhere match mode
        ///    LinkSessionId - Exact match mode
        /// </summary>
        public SearchSyncReportsAccountsStatisticsSearchByFields? SearchByField { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public SearchSyncReportsAccountsStatisticsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
