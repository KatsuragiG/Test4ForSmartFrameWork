using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Update symbols data for best trade opportunities report.
    /// </summary>
    public class UpdateBestTradeOpportunitiesSymbolContract
    {
        /// <summary>
        /// Best trade opportunities symbol Id
        /// </summary>
        public int BestTradeOpportunitiesSymbolId { get; set; }

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
