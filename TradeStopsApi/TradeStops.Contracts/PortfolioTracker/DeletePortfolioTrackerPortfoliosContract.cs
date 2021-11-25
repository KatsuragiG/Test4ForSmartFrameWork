using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to delete portfolio tracker portfolios.
    /// </summary>
    public class DeletePortfolioTrackerPortfoliosContract
    {
        /// <summary>
        /// List of portfolio Ids to delete.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Indicates whether Portfolio's event history must be deleted or not.
        /// </summary>
        public bool DeleteEventHistory { get; set; }
    }
}
