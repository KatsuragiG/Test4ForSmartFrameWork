using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Equity performance line point
    /// </summary>
    public class PortfolioPerformanceResultEquityContract
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Percent of change
        /// </summary>
        public decimal Equity { get; set; }
    }
}
