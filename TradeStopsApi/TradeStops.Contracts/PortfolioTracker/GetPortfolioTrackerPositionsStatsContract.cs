using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio tracker positions stats.
    /// </summary>
    public class GetPortfolioTrackerPositionsStatsContract
    {
        /// <summary>
        /// Ids of position for which statistics must be returned.
        /// </summary>
        public List<int> PositionIds { get; set; }

        /// <summary>
        /// Types of position statistics to return.
        /// </summary>
        public List<PositionStatsTypes> StatsTypes { get; set; }        
    }
}
