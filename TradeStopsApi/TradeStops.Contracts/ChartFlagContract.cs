using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Chart flag
    /// </summary>
    public class ChartFlagContract
    {
        /// <summary>
        /// Date of the chart flag item.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Flag value.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Flag name.
        /// </summary>
        public ChartFlagTypes FlagType { get; set; }
    }
}