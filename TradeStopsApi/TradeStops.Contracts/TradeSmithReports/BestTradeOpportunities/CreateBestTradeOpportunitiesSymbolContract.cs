using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create symbol for best trade opportunities report.
    /// </summary>
    public class CreateBestTradeOpportunitiesSymbolContract
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
