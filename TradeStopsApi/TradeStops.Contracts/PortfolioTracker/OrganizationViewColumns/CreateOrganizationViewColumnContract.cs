using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create (replace) organization view column (used in Finance website - Pubs (PortfolioTracker) section)
    /// </summary>
    public class CreateOrganizationViewColumnContract
    {
        /// <summary>
        /// Column type
        /// </summary>
        public ViewColumnTypes ColumnType { get; set; }

        /// <summary>
        /// Column width.
        /// </summary>
        public short Width { get; set; }

        /// <summary>
        ///  Number value of the column position in the grid.
        /// </summary>
        public short Sequence { get; set; }
    }
}
