using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create or update Yodlee User context
    /// </summary>
    public class CreateOrUpdateYodleeUserContextContract
    {
        /// <summary>
        /// Serialized user context.
        /// </summary>
        public string UserContext { get; set; }

        /// <summary>
        /// Context creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// (optional) Date when credentials were renewed last time.
        /// </summary>
        public DateTime? LastTouchConversationCredentials { get; set; }
    }
}
