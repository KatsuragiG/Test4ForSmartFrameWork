using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with percentage values for each Global Rank type for a given date.
    /// </summary>
    public class GlobalRankHistoricalAllocationContract
    {
        /// <summary>
        /// Trade date
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Strong bearish percent
        /// </summary>
        public decimal StrongBearishPercent { get; set; }

        /// <summary>
        /// Bearish percent
        /// </summary>
        public decimal BearishPercent { get; set; }

        /// <summary>
        /// Neutral percent
        /// </summary>
        public decimal NeutralPercent { get; set; }

        /// <summary>
        /// Bullish percent
        /// </summary>
        public decimal BullishPercent { get; set; }

        /// <summary>
        /// Strong bullish percent
        /// </summary>
        public decimal StrongBullishPercent { get; set; }
    }
}
