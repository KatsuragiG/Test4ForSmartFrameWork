using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to apply checklist
    /// </summary>
    public class ApplyChecklistContract
    {
        /// <summary>
        /// Checklist Id
        /// </summary>
        public int ChecklistId { get; set; }

        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
