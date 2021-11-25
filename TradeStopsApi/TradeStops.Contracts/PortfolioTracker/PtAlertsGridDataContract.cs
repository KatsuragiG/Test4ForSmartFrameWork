using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Alerts grid data
    /// </summary>
    public class PtAlertsGridDataContract
    {
        /// <summary>
        /// Array of Position Triggers, created for all Positions in requested Portfolios.
        /// </summary>
        public List<PtAlertsGridRowContract> Alerts { get; set; }
    }
}
