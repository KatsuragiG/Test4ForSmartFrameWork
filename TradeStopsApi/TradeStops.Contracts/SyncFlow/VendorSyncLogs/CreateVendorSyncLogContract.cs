using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Vendor sync log.
    /// </summary>
    public class CreateVendorSyncLogContract
    {
        /// <summary>
        ///  Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  Vendor sync error message ID.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }

        /// <summary>
        ///  (optional) Vendor error description.
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        ///  User email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Vendor user name (Yodlee) or request id (Plaid).
        /// </summary>
        public string VendorUsername { get; set; }

        /// <summary>
        ///  Yodlee service type.
        /// </summary>
        public SyncServiceTypes SyncServiceType { get; set; }

        /// <summary>
        ///  (optional) Last user login date.
        /// </summary>
        public DateTime? LastUserLoginDate { get; set; }

        /// <summary>
        ///  Is MFA broker.
        /// </summary>
        public bool IsMfa { get; set; }

        /// <summary>
        ///  Yodlee process type.
        /// </summary>
        public SyncProcessType SyncProcessType { get; set; }

        /// <summary>
        ///  TradeSmith user ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// Link session id
        /// </summary>
        public string LinkSessionId { get; set; }
    }
}
