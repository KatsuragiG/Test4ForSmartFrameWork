using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract to send list os symbol for bulk Position Size.
    /// </summary>
    public class BulkPositionSizeSymbolContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol trade type
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
