using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about subscription to display in zendesk widget
    /// </summary>
    public class ZendeskAppsSubscriptionInfoContract
    {
        /// <summary>
        /// The name of the subscription
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date when subscription is going to be expired.
        /// Can be null - it means that subscription has no expiration date (usual case for Lifetime subscription)
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}
