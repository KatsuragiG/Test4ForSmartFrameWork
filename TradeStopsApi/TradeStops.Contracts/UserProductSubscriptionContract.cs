using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information regarding User Product Subscription.
    /// Currently user can have only one (or zero) subscription record per each ProductSubscriptionId.
    /// This may be changed someday in the future to store the whole subscription history.
    /// </summary>
    public class UserProductSubscriptionContract
    {
        /// <summary>
        /// ID of the user's subscription.
        /// </summary>
        public int UserProductSubscriptionId { get; set; }

        /// <summary>
        /// Information regarding Product Subscription (subscription name, etc). 
        /// </summary>
        public ProductSubscriptionContract ProductSubscription { get; set; }

        /// <summary>
        /// ID of the Product Subscription.
        /// </summary>
        public ProductSubscriptions ProductSubscriptionId { get; set; }

        /// <summary>
        /// Subscription Status.
        /// This field must be used to check if user's subscription is active or not.
        /// Rely on 'Active' value, do not check 'Refunded' and 'Expired'.
        /// Do not check ExpirationDate additionally.
        /// </summary>
        public SubscriptionStatuses SubscriptionStatusId { get; set; }

        /// <summary>
        /// The date when subscription was added to user's account for the first time.
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// The date when subscription was expired or going to be expired.
        /// Can be null for active subscriptions without expiration date (like Lifetime subscriptions).
        /// Not null for all inactive subscriptions.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// The source of subscription.
        /// 'TradeStops' means that it was manually created in Admin Area
        /// or it was created by internal code (usually for tracking purposes).
        /// </summary>
        public SyncronizationSources SyncSourceId { get; set; }

        /// <summary>
        /// Arb means 'Automatic recurring billing' or something similar.
        /// Field seems to be obsolete.
        /// </summary>
        public bool? ArbEnabled { get; set; }

        /// <summary>
        /// ID of the marketing campaign that was used to sell subscription.
        /// </summary>
        public string CampaignId { get; set; }
    }
}
