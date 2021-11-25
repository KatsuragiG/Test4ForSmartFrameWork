using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create TradeSmith user
    /// </summary>
    public class CreateTradeSmithUserContract
    {
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// User agora customer number
        /// </summary>
        public string AgoraCustomerNumber { get; set; }

        /// <summary>
        /// User snaid
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// User personal info
        /// </summary>
        public UserPersonalInfoContract PersonalInfo { get; set; }
    }
}
