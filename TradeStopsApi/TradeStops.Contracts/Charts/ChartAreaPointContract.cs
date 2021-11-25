using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Data for the chart area item.
    /// </summary>
    public class ChartAreaPointContract
    {
        /// <summary>
        /// Date of the chart area item.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Upper boundary of the chart area item.
        /// </summary>
        public decimal UpperBound { get; set; }

        /// <summary>
        /// Lower boundary the chart area item.
        /// </summary>
        public decimal LowerBound { get; set; }
    }
}
