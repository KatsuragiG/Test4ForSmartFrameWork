using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Data to display in chart
    /// </summary>
    public class BacktesterSubtaskResultChartPointContract
    {
        /// <summary>
        /// Trading day. Doesn't include trading holidays.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Absolute gain value (in dollars or other currency)
        /// </summary>
        public decimal DollarGain { get; set; }

        /// <summary>
        /// Gain change in percents. Value 100 corresponds to 100%.
        /// </summary>
        public decimal PercentGain { get; set; }
    }
}
