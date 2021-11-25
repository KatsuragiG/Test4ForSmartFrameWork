using System.Collections.Generic;

namespace TradeStops.Contracts.Generic
{
    /// <summary>
    /// Data for grid with additional info.
    /// </summary>
    /// <typeparam name="TResult">Type of items to return for grid.</typeparam>
    /// <typeparam name="TAdditional">Type of additional parameters to return with the grid.</typeparam>
    public class SearchResultWithAdditionalInfoContract<TResult, TAdditional>
    {
        /// <summary>
        /// Total items in the grid.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// List of paginated items in the grid.
        /// </summary>
        public IEnumerable<TResult> Items { get; set; }

        /// <summary>
        /// The value contains additional parameters to return with the grid.
        /// </summary>
        public TAdditional AdditionalInfo { get; set; }
    }
}
