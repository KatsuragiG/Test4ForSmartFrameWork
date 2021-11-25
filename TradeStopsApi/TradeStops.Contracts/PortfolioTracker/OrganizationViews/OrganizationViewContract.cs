using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Organization view (used in PortfolioTracker grids)
    /// </summary>
    public class OrganizationViewContract
    {
        /// <summary>
        /// ID of the Organization View.
        /// </summary>
        public int OrganizationViewId { get; set; }

        /// <summary>
        /// View type (PortfolioTrackerAlerts, PortfolioTrackerPositions).
        /// </summary>
        public ViewTypes ViewType { get; set; }

        /// <summary>
        /// User view name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column that is used to sort results.
        /// </summary>
        public ViewColumnTypes SortColumn { get; set; }

        /// <summary>
        /// Type of the sorting direction for the column that is used to sort (Ascending, Descending).
        /// </summary>
        public SortTypes SortType { get; set; }
    }
}
