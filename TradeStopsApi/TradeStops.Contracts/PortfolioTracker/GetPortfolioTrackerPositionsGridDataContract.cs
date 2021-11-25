using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio tracker position grid data.
    /// </summary>
    public class GetPortfolioTrackerPositionsGridDataContract
    {
        /// <summary>
        /// Portfolio Ids list.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Type of portfolio statistics to return.
        /// </summary>
        public PortfolioStatsTypes PortfolioStatsType { get; set; }
    }
}
