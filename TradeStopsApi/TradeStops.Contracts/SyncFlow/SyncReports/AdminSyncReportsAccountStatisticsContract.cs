using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Vendor Account in the sync reports of sync reports.
    /// </summary>
    public class AdminSyncReportsAccountStatisticsContract
    {
        /// <summary>
        ///  Vendor sync log ID.
        /// </summary>
        public int VendorSyncLogId { get; set; }

        /// <summary>
        ///  Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Vendor username.
        /// </summary>
        public string VendorUsername { get; set; }

        /// <summary>
        ///  Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  Is successful vendor sync log.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        ///  Sync error code.
        /// </summary>
        public YodleeErrorCodeTypes? ErrorCode { get; set; }

        /// <summary>
        ///  Sync error name.
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        ///  Sync error description.
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        ///  Vendor sync service type.
        /// </summary>
        public SyncServiceTypes SyncServiceType { get; set; }

        /// <summary>
        ///  Vendor sync process type.
        /// </summary>
        public SyncProcessType SyncProcessType { get; set; }

        /// <summary>
        ///  Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  TradeSmith User ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        ///  User email.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Vendor sync log creation date in UTC.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///  Last successful refresh date.
        /// </summary>
        public DateTime? LastSuccessDate { get; set; }

        /// <summary>
        ///  Difference between Date and LastSuccessDate in days.
        /// </summary>
        public int? DaysNotSynched { get; set; }

        /// <summary>
        /// Link session id
        /// </summary>
        public string LinkSessionId { get; set; }
    }
}
