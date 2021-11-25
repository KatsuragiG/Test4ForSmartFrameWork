using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Import process
    /// </summary>
    public class ImportProcessContract
    {
        /// <summary>
        ///  Import process ID.
        /// </summary>
        public int ImportProcessId { get; set; }

        /// <summary>
        /// Import process status.
        /// </summary>
        public ImportProcessStatusTypes ImportProcessStatusType { get; set; }

        /// <summary>
        /// Import process progress.
        /// </summary>
        public ImportProgressTypes ImportProgressType { get; set; }

        /// <summary>
        /// TradeSmith User ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        /// Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        /// Import date.
        /// </summary>
        public DateTime ImportDate { get; set; }

        /// <summary>
        /// Sync error message ID.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }

        /// <summary>
        /// Application type initiated import process.
        /// </summary>
        public SyncFlowModuleAppTypes SyncFlowModuleAppType { get; set; }

        /// <summary>
        /// Defines is user informed about the import process.
        /// </summary>
        public bool IsUserInformed { get; set; }

        /// <summary>
        /// Error description.
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Defines is portfolio limit exceeded.
        /// </summary>
        public bool PortfoliosLimitExceeded { get; set; }
    }
}
