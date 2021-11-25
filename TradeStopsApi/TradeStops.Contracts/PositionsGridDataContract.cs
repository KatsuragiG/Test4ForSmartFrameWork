using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// TradeStops positions grid data
    /// </summary>
    public class PositionsGridDataContract
    {
        /// <summary>
        /// Positions array of requested portfolios.
        /// </summary>
        public List<PositionsGridRowContract> Positions { get; set; }

        /// <summary>
        /// UnconfirmedPositions
        /// </summary>
        public List<UnconfirmedPositionsGridRowContract> UnconfirmedPositions { get; set; }

        /// <summary>
        /// Total values of requested portfolios.
        /// </summary>
        public PortfolioTotalContract PortfolioTotal { get; set; }
    }
}
