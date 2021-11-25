using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search user
    /// </summary>
    public class SearchTradeSmithUsersContract
    {
        /// <summary>
        /// Value to search for
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Field to search for
        /// </summary>
        public SearchUserFields? SearchField { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Field to order by
        /// </summary>
        public SearchUserFields? OrderField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes? OrderType { get; set; }
    }
}
