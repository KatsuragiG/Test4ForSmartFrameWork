using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create symbols data for best trade opportunities report.
    /// </summary>
    public class BulkCreateBestTradeOpportunitiesSymbolsContract
    {
        /// <summary>
        /// Draft report history Id
        /// </summary>
        public int DraftReportHistoryId { get; set; }

        /// <summary>
        /// Best trade opportunities symbols
        /// </summary>
        public List<CreateBestTradeOpportunitiesSymbolContract> BestTradeOpportunitiesSymbols { get; set; }
    }
}
