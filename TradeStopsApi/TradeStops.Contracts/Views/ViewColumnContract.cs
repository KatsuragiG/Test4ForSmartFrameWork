using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// View column (used in TradeStops grids and in PortfolioTracker grids).
    /// Represent view columns as entity that is displayed on UI while user selecting the necessary columns to show.
    /// </summary>
    public class ViewColumnContract
    {
        /// <summary>
        /// The type of the column (Symbol, Vq, EntryPrice, etc).
        /// Part of the Alternate (Composite) Key.
        /// </summary>
        public ViewColumnTypes ColumnType { get; set; }

        /// <summary>
        /// View type (Positions, Alerts, Closed Positions).
        /// Part of the Alternate (Composite) Key.
        /// </summary>
        public ViewTypes ViewType { get; set; }

        /// <summary>
        /// Column group name.
        /// </summary>
        public ColumnGroups ColumnGroup { get; set; }

        /// <summary>
        /// True for columns that belong to Default View. Used on UI to reset checked columns to default set.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Current column width.
        /// </summary>
        public short DefaultWidth { get; set; }

        /// <summary>
        /// Number value of the column position in the grid.
        /// </summary>
        public short? DefaultSequence { get; set; }
    }
}
