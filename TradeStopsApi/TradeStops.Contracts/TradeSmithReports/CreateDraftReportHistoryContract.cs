using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create draft report history for generating report
    /// </summary>
    public class CreateDraftReportHistoryContract
    {
        /// <summary>
        /// Report type
        /// </summary>
        public ReportTypes ReportTypeId { get; set; }

        /// <summary>
        /// Draft data for Best Trade Opportunities email
        /// </summary>
        public List<CreateBestTradeOpportunitiesSymbolContract> BestTradeOpportunitiesSymbols { get; set; }

        /// <summary>
        /// Draft data for Monthly Opportunities  email
        /// </summary>
        public List<CreateMonthlyOpportunitiesSymbolContract> MonthlyOpportunitiesSymbols { get; set; }
    }
}
