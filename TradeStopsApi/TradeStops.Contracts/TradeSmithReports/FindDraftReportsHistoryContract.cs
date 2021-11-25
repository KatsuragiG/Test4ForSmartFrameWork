using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Find draft reports history
    /// </summary>
    public class FindDraftReportsHistoryContract
    {
        /// <summary>
        /// Report type
        /// </summary>
        public ReportTypes? ReportTypeId { get; set; }

        /// <summary>
        /// From date (UTC)
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// To date (UTC)
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
