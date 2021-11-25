using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contains an authentication token for the Sync Flow module.
    /// </summary>
    public class SyncFlowAuthenticationTokenContract
    {
        /// <summary>
        /// Authentication token.
        /// </summary>
        public Guid AuthenticationToken { get; set; }
    }
}
