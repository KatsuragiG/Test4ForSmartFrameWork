using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Historical SSI value
    /// </summary>
    public class SsiHistoryValueContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade date
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// SSI status for Long position adjusted by dividends, splits, stock distributions, spin-offs
        /// </summary>
        public SsiStatuses LongAdj { get; set; }

        /// <summary>
        /// SSI status for Long position (not adjusted by dividends, adjusted by splits, stock distributions, spin-offs)
        /// </summary>
        public SsiStatuses Long { get; set; }

        /// <summary>
        /// Ssi status for Short position (not adjusted by dividends, adjusted by splits, stock distributions, spin-offs)
        /// </summary>
        public SsiStatuses Short { get; set; }
    }
}
