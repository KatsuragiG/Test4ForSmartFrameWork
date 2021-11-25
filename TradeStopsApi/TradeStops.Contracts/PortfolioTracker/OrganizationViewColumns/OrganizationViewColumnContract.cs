using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Represent view columns that are used by specific organization.
    /// Used in Finance for PortfolioTracker grids.
    /// </summary>
    public class OrganizationViewColumnContract
    {
        /// <summary>
        /// ID of the view where this column is used.
        /// </summary>
        public int OrganizationViewId { get; set; }

        /// <summary>
        /// The type of the column (Symbol, Vq, EntryPrice, etc).
        /// </summary>
        public ViewColumnTypes ColumnType { get; set; }

        /// <summary>
        /// Column width (in pixels).
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// Column position in the grid.
        /// </summary>
        public short Sequence { get; set; }
    }
}
