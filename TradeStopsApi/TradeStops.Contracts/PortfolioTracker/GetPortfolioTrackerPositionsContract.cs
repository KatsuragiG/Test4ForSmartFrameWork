using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio tracker positions.
    /// </summary>
    public class GetPortfolioTrackerPositionsContract
    {
        /// <summary>
        /// (Optional) Find positions matching provided positionsIds.
        /// </summary>
        public List<int> PositionIds { get; set; }

        /// <summary>
        /// (Optional) Find positions matching provided portfolioIds. If no ids were provided, than positions from all user portfolios will be returned.
        /// </summary>
        public List<int> PortfolioIds { get; set; }        
    }
}
