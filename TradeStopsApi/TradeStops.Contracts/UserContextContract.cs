using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// User context
    /// </summary>
    public class UserContextContract
    {
        /// <summary>
        /// User Context key is required for authorization for user-specific actions, like /portfolios, /positions
        /// </summary>
        public Guid ContextKey { get; set; }
    }
}
