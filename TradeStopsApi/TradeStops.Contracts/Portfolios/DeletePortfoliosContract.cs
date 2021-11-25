using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio deletion options
    /// </summary>
    public class DeletePortfoliosContract
    {
        /// <summary>
        /// Portfolio IDs.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Delete the event history of the portfolio as well.
        /// </summary>
        public bool DeleteWithHistory { get; set; }
    }
}
