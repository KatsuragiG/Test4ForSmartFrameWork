using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get historical SSI Trend distribution for a list of portfolios
    /// </summary>
    public class GetHistoricalSsiTrendDistributionByPortfolioIdsContract
    {
        /// <summary>
        /// Return distribution for positions from provided portfolio ids.
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
