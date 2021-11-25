using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio Tracker positions grid data
    /// </summary>
    public class PortfolioTrackerPositionsGridDataContract
    {
        /// <summary>
        /// Positions array of requested portfolios.
        /// </summary>
        public List<PortfolioTrackerPositionsGridRowContract> Positions { get; set; }
    }
}
