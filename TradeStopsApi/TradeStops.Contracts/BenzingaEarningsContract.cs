using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Benzinga earnings data
    /// </summary>
    public class BenzingaEarningsContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date and time of data update for the report.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Quarter for which the report was received.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Year for which the report was received.
        /// </summary>
        public int PeriodYear { get; set; }

        /// <summary>
        /// Earnings per share.
        /// </summary>
        public double? Eps { get; set; }

        /// <summary>
        /// Estimated earnings per share.
        /// </summary>
        public double? EpsEstimated { get; set; }

        /// <summary>
        /// Difference between estimated and actual earnings per share.
        /// </summary>
        public double? EpsSurprise { get; set; }
    }
}
