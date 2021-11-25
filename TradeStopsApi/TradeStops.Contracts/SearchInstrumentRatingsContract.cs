using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search instrument ratings.
    /// </summary>
    public class SearchInstrumentRatingsContract
    {
        /// <summary>
        /// Order by field.
        /// </summary>
        public SearchInstrumentRatingsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }

        /// <summary>
        /// Maximal number of instrument ratings.
        /// </summary>
        public int MaxResults { get; set; }
    }
}
