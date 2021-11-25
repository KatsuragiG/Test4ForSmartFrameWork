using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for search users by subscription.
    /// </summary>
    public class SearchTradeSmithUsersBySubscriptionContract
    {
        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///  Search by subscription id.
        /// </summary>
        public ProductSubscriptions SubscriptionId { get; set; }
    }
}
