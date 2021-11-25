using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Column that is added to user's view in TradeStops grid.
    /// </summary>
    public class UserViewColumnContract
    {
        /// <summary>
        /// ID of the view where this column is used.
        /// </summary>
        public int UserViewId { get; set; }

        /// <summary>
        /// The type of the column (Symbol, Vq, EntryPrice, etc).
        /// Part of the Alternate (Composite) Key.
        /// </summary>
        public ViewColumnTypes ColumnType { get; set; }

        /// <summary>
        /// Current column width.
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        /// Number value of the column position in the grid.
        /// </summary>
        public short Sequence { get; set; }
    }
}
