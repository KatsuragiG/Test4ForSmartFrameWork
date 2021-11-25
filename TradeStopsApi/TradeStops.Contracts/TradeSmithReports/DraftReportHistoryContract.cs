using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Draft reports hictory
    /// </summary>
    public class DraftReportHistoryContract
    {
        /// <summary>
        /// Draft report rule Id
        /// </summary>
        public int DraftReportHistoryId { get; set; }

        /// <summary>
        /// Report type
        /// </summary>
        public ReportTypes ReportTypeId { get; set; }

        /// <summary>
        /// Create date (UTC)
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Update date (UTC)
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Send date (UTC)
        /// </summary>
        public DateTime? SendDate { get; set; }
    }
}
