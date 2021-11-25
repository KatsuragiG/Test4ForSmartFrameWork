using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Get shaids and agora customer numbers
    /// </summary>
    public class AdditionalNewsletterAccountsContract
    {
        /// <summary>
        /// Snaids for user
        /// </summary>
        public List<string> Snaids { get; set; }

        /// <summary>
        /// Agora customer numbers for user
        /// </summary>
        public List<string> AgoraCustomerNumbers { get; set; }
    }
}
