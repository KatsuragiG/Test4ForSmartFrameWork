using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create user view (used in TradeStops website)
    /// </summary>
    public class CreateUserViewContract
    {
        /// <summary>
        /// Name of the new view.
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

        /// <summary>
        /// View type.
        /// </summary>
        public ViewTypes ViewType { get; set; }
    }
}
