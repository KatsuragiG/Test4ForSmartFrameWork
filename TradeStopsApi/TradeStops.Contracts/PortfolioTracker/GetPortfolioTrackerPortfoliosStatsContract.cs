using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio tracker portfolios statistics
    /// </summary>
    public class GetPortfolioTrackerPortfoliosStatsContract
    {
        /// <summary>
        /// Ids of portfolios for which statistics must be returned.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Types of portfolio statistics to return.
        /// </summary>
        public List<PortfolioStatsTypes> PortfolioStatsTypes { get; set; }
    }
}
