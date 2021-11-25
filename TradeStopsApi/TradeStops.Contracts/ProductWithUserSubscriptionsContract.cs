using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with information about TradeSmith Product with user subscriptions for product
    /// </summary>
    public class ProductWithUserSubscriptionsContract
    {
        /// <summary>
        /// Information about product
        /// </summary>
        public ProductContract Product { get; set; }

        /// <summary>
        /// List of user subscriptions for product, including inactive
        /// </summary>
        public List<UserProductSubscriptionContract> UserProductSubscriptions { get; set; }

        /// <summary>
        /// Active subscription with highest weight if user is active,
        /// or inactive subscription with highest expiration date if user is expired,
        /// or 'null' if user was never subscribed to project
        /// </summary>
        public UserProductSubscriptionContract CurrentSubscription { get; set; }

        /// <summary>
        /// Indicates if user has any active subscription for product
        /// </summary>
        public bool IsUserSubscribed { get; set; }
    }
}
