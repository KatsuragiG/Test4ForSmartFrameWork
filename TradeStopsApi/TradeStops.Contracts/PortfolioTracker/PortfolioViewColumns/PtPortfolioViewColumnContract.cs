using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Represent view columns that are used when publishing Portfolio in PT2 (Pubs)
    /// or when displaying portfolio (Finance website Gurus section).
    /// </summary>
    public class PtPortfolioViewColumnContract
    {
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
