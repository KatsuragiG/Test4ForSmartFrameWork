using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit Portfolio view
    /// </summary>
    public class EditPtPortfolioViewContract
    {
        /// <summary>
        /// Column that is used to sort results.
        /// </summary>
        public Optional<ViewColumnTypes> SortColumn { get; set; }

        /// <summary>
        /// Type of the sorting direction for the column that is used to sort (Ascending, Descending).
        /// </summary>
        public Optional<SortTypes> SortType { get; set; }
    }
}
