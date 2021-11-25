using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Copy positions parameters
    /// </summary>
    public class CopyPositionsContract
    {
        /// <summary>
        /// Target portfolio ID.
        /// </summary>
        public int TargetPortfolioId { get; set; }

        /// <summary>
        /// Array of position Ids to copy.
        /// </summary>
        public List<int> PositionIds { get; set; }

        /// <summary>
        /// Value defines whether alerts will be copied together with positions.
        /// </summary>
        public bool CopyAlerts { get; set; }
    }
}
