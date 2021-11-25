using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create vendor username.
    /// </summary>
    public class CreateVendorUsernameContract
    {
        /// <summary>
        /// Encrypted vendor username.
        /// </summary>
        public string VendorUsername { get; set; }

        /// <summary>
        /// Encrypted vendor password
        /// </summary>
        public string VendorPassword { get; set; }

        /// <summary>
        /// Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }
    }
}
