using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Update symbols data for Monthly Opportunities report.
    /// </summary>
    public class UpdateMonthlyOpportunitiesSymbolContract
    {
        /// <summary>
        /// Monthly opportunities symbol Id
        /// </summary>
        public int MonthlyOpportunitiesSymbolId { get; set; }

        /// <summary>
        /// Report section type Id
        /// </summary>
        public Optional<ReportSectionTypes> ReportSectionTypeId { get; set; }

        /// <summary>
        /// Symbol Id
        /// </summary>
        public Optional<int> SymbolId { get; set; }
    }
}
