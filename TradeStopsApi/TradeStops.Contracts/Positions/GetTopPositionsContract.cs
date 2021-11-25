using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to load positions for selected portfolios.
    /// </summary>
    public class GetTopPositionsContract
    {
        /// <summary>
        /// List of portfolio IDs to load positions.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Type of statistics to find positions, like TopWinners (max gain), TopLosers (min gain).
        /// </summary>
        public TopStatisticsTypes TopStatisticsTypes { get; set; }

        /// <summary>
        /// Number of items to return from the top of the resulting list.
        /// </summary>
        public int TopItemsCount { get; set; }
    }
}
