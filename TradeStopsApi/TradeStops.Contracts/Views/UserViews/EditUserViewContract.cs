using System.Collections.Generic;

using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit user view
    /// </summary>
    public class EditUserViewContract
    {
        /// <summary>
        /// (optional) Name of the view to be edited.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) Column that is used to sort results.
        /// </summary>
        public Optional<ViewColumnTypes> SortColumn { get; set; }

        /// <summary>
        /// (optional) Type of the sorting direction for the column that is used to sort (Ascending, Descending).
        /// </summary>
        public Optional<SortTypes> SortType { get; set; }
    }
}
