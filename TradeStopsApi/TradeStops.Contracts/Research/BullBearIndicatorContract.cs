using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract contains Bull and Bear indicator for the symbol.
    /// </summary>
    public class BullBearIndicatorContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Indicator trigger date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Indicator status.
        /// </summary>
        public BullBearIndicatorStatuses Indicator { get; set; }
    }
}
