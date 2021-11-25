using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbols data for Best Trade Opportunities report
    /// </summary>
    public class BestTradeOpportunitiesSymbolContract
    {
        /// <summary>
        /// Best trade opportunities symbolId
        /// </summary>
        public int BestTradeOpportunitiesSymbolId { get; set; }

        /// <summary>
        /// Draft report history Id
        /// </summary>
        public int DraftReportHistoryId { get; set; }

        /// <summary>
        /// Report section type
        /// </summary>
        public ReportSectionTypes ReportSectionTypeId { get; set; }

        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol name
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Option type
        /// </summary>
        public OptionTypes OptionType { get; set; }
    }
}
