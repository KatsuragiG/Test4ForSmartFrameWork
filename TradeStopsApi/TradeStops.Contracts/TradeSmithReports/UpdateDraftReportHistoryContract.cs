using System;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Update draft report history
    /// </summary>
    public class UpdateDraftReportHistoryContract
    {
        /// <summary>
        /// Draft report history Id
        /// </summary>
        public int DraftReportHistoryId { get; set; }

        /// <summary>
        /// Send date (UTC)
        /// </summary>
        public Optional<DateTime> SendDate { get; set; }
    }
}
