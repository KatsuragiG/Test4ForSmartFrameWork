using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// User view (used in TradeStops grids)
    /// </summary>
    public class UserViewContract
    {
        /// <summary>
        /// User view Id.
        /// </summary>
        public int UserViewId { get; set; }

        /// <summary>
        /// View type (Positions, Closed Positions, Alerts).
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
