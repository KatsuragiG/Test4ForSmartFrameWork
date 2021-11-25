using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Vendor system error
    /// </summary>
    public class VendorSystemErrorContract
    {
        /// <summary>
        ///  Vendor system error ID.
        /// </summary>
        public int VendorSystemErrorId { get; set; }

        /// <summary>
        ///  Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  User email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///  Vendor system error occurence date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///  TradeSmith user ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        ///  Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }
    }
}
