using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit Vendor sync log.
    /// </summary>
    public class EditVendorSyncLogContract
    {
        /// <summary>
        ///  (optional) Vendor sync error message ID.
        /// </summary>
        public Optional<int> VendorSyncErrorMessageId { get; set; }

        /// <summary>
        /// (optional) Vendor type.
        /// </summary>
        public Optional<VendorTypes?> VendorType { get; set; }

        /// <summary>
        ///  (optional) Vendor error description.
        /// </summary>
        public Optional<string> ErrorDescription { get; set; }

        /// <summary>
        /// Vendor user name (Yodlee) or request id (Plaid). Change is possible if the current value is null.
        /// </summary>
        public Optional<string> VendorUsername { get; set; }
    }
}
