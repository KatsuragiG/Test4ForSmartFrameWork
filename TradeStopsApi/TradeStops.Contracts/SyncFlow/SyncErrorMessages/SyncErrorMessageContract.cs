using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Sync server error message
    /// </summary>
    public class SyncErrorMessageContract
    {
        /// <summary>
        /// Vendor error unique ID.
        /// </summary>
        public int VendorSyncErrorMessageId { get; set; }

        /// <summary>
        ///  Yodlee error code.
        /// </summary>
        public YodleeErrorCodeTypes? ErrorCode { get; set; }

        /// <summary>
        ///  Sync error type.
        /// </summary>
        public SyncErrorTypes ErrorType { get; set; }

        /// <summary>
        ///  Sync error name.
        /// </summary>
        public string ErrorName { get; set; }

        /// <summary>
        ///  Sync error description.
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        ///  Defines are new credentials required.
        /// </summary>
        public bool IsRequiredNewCredentials { get; set; }

        /// <summary>
        ///  Defines is info popup required.
        /// </summary>
        public bool IsRequiredInfoPopup { get; set; }

        /// <summary>
        /// Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }
    }
}
