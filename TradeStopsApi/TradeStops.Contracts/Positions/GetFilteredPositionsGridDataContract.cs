using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get opened positions.
    /// </summary>
    public class GetFilteredPositionsGridDataContract
    {
        /// <summary>
        /// Return positions from provided portfolios.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific symbol types.
        /// </summary>
        public List<SymbolTypes> SymbolTypes { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific symbol types.
        /// </summary>
        public List<TradeTypes> TradeTypes { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific health types.
        /// </summary>
        public List<SsiStatuses> HealthTypes { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific status types.
        /// </summary>
        public List<PositionStatusFilterTypes> PositionStatusTypes { get; set; }

        /// <summary>
        /// (Optional) Return positions with with issues (incomplete data, delisted, symbol not recognized etc).
        /// </summary>
        public List<PositionGridIssueTypes> PositionGridIssues { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific alert status types.
        /// </summary>
        public List<PositionAlertStatusTypes> AlertStatusTypes { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific alert status types.
        /// </summary>
        public DecimalFilter GainWithDivFilter { get; set; }

        /// <summary>
        /// (Optional) Return positions with specific alert status types.
        /// </summary>
        public DecimalFilter VqFilter { get; set; }

        /// <summary>
        /// (Optional) Return positions from portfolios with specific sync types.
        /// </summary>
        public List<PortfolioSyncTypes> PortfolioSyncTypes { get; set; }
    }
}
