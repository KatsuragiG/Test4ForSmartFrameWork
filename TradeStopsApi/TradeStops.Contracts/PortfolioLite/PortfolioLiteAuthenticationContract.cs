using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract for authentication in the portfolio lite
    /// </summary>
    public class PortfolioLiteAuthenticationContract
    {
        /// <summary>
        /// Unique Stansberry identifier
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// Portfolio lite key provided to partners
        /// </summary>
        public Guid PortfolioLitePartnerKey { get; set; }
    }
}
