using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for patching a user subscription
    /// </summary>
    public class PatchUserSubscriptionContract
    {
        /// <summary>
        /// (Optional) Product subscription Id
        /// </summary>
        public Optional<ProductSubscriptions> ProductSubscriptionId { get; set; }

        /// <summary>
        /// (Optional) Expiration date. Subscription status is determined automatically by this field.
        /// </summary>
        public Optional<DateTime?> ExpirationDate { get; set; }
    }
}
