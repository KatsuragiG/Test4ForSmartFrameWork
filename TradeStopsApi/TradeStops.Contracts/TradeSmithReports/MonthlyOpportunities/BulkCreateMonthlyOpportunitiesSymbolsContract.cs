using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create symbols data for Monthly Opportunities report.
    /// </summary>
    public class BulkCreateMonthlyOpportunitiesSymbolsContract
    {
        /// <summary>
        /// Draft report history Id
        /// </summary>
        public int DraftReportHistoryId { get; set; }

        /// <summary>
        /// Monthly Opportunities symbols
        /// </summary>
        public List<CreateMonthlyOpportunitiesSymbolContract> MonthlyOpportunitiesSymbols { get; set; }
    }
}
