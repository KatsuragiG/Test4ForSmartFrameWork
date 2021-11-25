using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create symbol for Monthly Opportunities report.
    /// </summary>
    public class CreateMonthlyOpportunitiesSymbolContract
    {
        /// <summary>
        /// Report section type
        /// </summary>
        public ReportSectionTypes ReportSectionTypeId { get; set; }

        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }
    }
}
