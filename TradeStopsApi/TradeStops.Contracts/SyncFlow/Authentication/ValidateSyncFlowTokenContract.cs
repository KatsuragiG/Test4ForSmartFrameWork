using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contains the result of the token validation.
    /// </summary>
    public class ValidateSyncFlowTokenContract
    {
        /// <summary>
        /// Token owner guid.
        /// </summary>
        public Guid UserGuid { get; set; }
    }
}
