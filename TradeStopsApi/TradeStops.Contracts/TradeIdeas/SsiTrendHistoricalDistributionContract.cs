using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with percentage values for each SSI Trend status for a given date.
    /// </summary>
    public class SsiTrendHistoricalDistributionContract
    {
        /// <summary>
        /// Trade date
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// SSI up-trend percent
        /// </summary>
        public decimal UpTrendPercent { get; set; }

        /// <summary>
        /// SSI side-trend percent
        /// </summary>
        public decimal SideTrendPercent { get; set; }

        /// <summary>
        /// SSI down-trend percent
        /// </summary>
        public decimal DownTrendPercent { get; set; }
    }
}