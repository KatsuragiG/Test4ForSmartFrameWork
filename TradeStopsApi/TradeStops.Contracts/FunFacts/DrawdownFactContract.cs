using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Drawdown fun fact parameters
    /// </summary>
    public class DrawdownFactContract
    {
        /// <summary>
        /// Fall start date.
        /// </summary>
        public DateTime FallStartDate { get; set; }

        /// <summary>
        /// Recovery date.
        /// </summary>
        public DateTime? RecoveryDate { get; set; }

        /// <summary>
        /// Number of days of stock drawdown.
        /// </summary>
        public int DrawdownPeriod { get; set; }

        /// <summary>
        /// Percentage of fall between the date of the max point and the min point.
        /// </summary>
        public decimal FallPercent { get; set; }
    }
}
