using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Market Outlook data
    /// </summary>
    public class MarketOutlookContract
    {
        /// <summary>
        /// The type of market outlook
        /// </summary>
        public MarketOutlookTypes MarketOutlookId { get; set; }

        /// <summary>
        /// The name of the index
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Market outlook group Id
        /// </summary>
        public MarketOutlookGroupIds? MarketOutlookGroupId { get; set; }

        /// <summary>
        /// Symbol group type
        /// </summary>
        public SymbolGroupTypes SymbolGroupType { get; set; }
    }
}
