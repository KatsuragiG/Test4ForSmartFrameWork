using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for edit user
    /// </summary>
    public class EditTradeSmithUserContract
    {
        /// <summary>
        /// User shaid
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// User agora customer number
        /// </summary>
        public string AgoraCustomerNumber { get; set; }

        /// <summary>
        /// User notes
        /// </summary>
        public string Notes { get; set; }
    }
}
