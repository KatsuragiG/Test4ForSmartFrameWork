using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get historical Global Rank allocation for a list of portfolios
    /// </summary>
    public class GetHistoricalGlobalRankAllocationContract
    {
        /// <summary>
        /// Return allocation for positions from provided portfolio ids.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Calculate distribution from this date.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Calculate distribution to this date.
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
