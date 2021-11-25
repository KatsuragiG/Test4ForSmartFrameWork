using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create import process. Used in portfolio synchronization
    /// </summary>
    public class CreateImportProcessContract
    {
        /// <summary>
        ///  Vendor Account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        ///  (optional) Import process status.
        /// </summary>
        public ImportProcessStatusTypes? ImportProcessStatusType { get; set; }

        /// <summary>
        ///  Vendor sync error message ID.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }

        /// <summary>
        /// Application type initiated import process.
        /// </summary>
        public SyncFlowModuleAppTypes SyncFlowModuleAppType { get; set; }

        /// <summary>
        ///  (optional) Error description.
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}
