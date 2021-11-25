using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get general statistics for sync reports.
    /// </summary>
    public class GetSyncReportsGeneralStatisticsContract
    {
        /// <summary>
        ///  Synchronization vendor type filter.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  General statistics range from that date in UTC.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///  General statistics range to that date in UTC.
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
