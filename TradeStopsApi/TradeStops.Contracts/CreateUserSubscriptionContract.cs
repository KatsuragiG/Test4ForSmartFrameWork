using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a user subscription
    /// </summary>
    public class CreateUserSubscriptionContract
    {
        /// <summary>
        /// Product subscription Id
        /// </summary>
        public ProductSubscriptions ProductSubscriptionId { get; set; }

        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
    }
}
