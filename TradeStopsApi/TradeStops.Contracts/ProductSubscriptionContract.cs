using TradeStops.Common.Enums;

// todo: reorganize contracts to have all product-subscriptions-related stuff in folder ProductSubscriptions. Consider to reorganize other contracts also
namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about TradeSmith product subscription
    /// </summary>
    public class ProductSubscriptionContract
    {
        /// <summary>
        /// ID of the product subscription
        /// </summary>
        public ProductSubscriptions ProductSubscriptionId { get; set; }

        /// <summary>
        /// ID of the product
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// The name of subscription
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Weight of the subscription
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Corresponding pub-code for subscriptions that are synchronized with Stansberry.
        /// Can be null if subscription doesn't exist in Stansberry, like 'PortfolioLite'
        /// Obsolete property that shouldn't be used!
        /// </summary>
        public string StansberryPubCode { get; set; }
    }
}
