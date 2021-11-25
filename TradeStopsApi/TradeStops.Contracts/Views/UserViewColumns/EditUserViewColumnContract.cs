using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit user view column.
    /// </summary>
    public class EditUserViewColumnContract
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
