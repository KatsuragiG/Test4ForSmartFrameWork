using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Alerts grid data
    /// </summary>
    public class AlertsGridDataContract
    {
        /// <summary>
        /// Array of Position Triggers, created for all Positions in requested Portfolios.
        /// </summary>
        public List<AlertsGridRowContract> Alerts { get; set; }

        /// <summary>
        /// Some basic information about Portfolio, like positions count, gains, etc
        /// </summary>
        public PortfolioTotalContract PortfolioTotal { get; set; }

        /// <summary>
        /// Total number of opened positions in all Portfolios.
        /// </summary>
        public int TotalOpenedPositions { get; set; }

        /// <summary>
        /// Internal TradeStops value. Total number of unconfirmed positions in all Portfolios. Unconfirmed positions can be created only during synchronization with brokerage.
        /// </summary>
        public int TotalUnconfirmedPositions { get; set; }
    }
}
