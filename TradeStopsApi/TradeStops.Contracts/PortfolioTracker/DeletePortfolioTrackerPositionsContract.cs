using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to delete portfolio tracker positions.
    /// </summary>
    public class DeletePortfolioTrackerPositionsContract
    {
        /// <summary>
        /// List of position Ids to delete.
        /// </summary>
        public List<int> PositionIds { get; set; }        
    }
}
