using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit portfolio view column.
    /// </summary>
    public class EditPtPortfolioViewColumnContract
    {
        /// <summary>
        /// Column width (in pixels).
        /// </summary>
        public Optional<short> Width { get; set; }

        /////// <summary>
        /////// Column position in the grid.
        /////// </summary>
        ////public short Sequence { get; set; }
    }
}
