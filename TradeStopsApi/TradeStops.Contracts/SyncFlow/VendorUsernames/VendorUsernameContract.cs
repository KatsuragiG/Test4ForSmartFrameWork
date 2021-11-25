using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Vendor username required for login.
    /// </summary>
    public class VendorUsernameContract
    {
        /// <summary>
        /// Vendor username ID.
        /// </summary>
        public int VendorUsernameId { get; set; }

        /// <summary>
        /// TradeSmith user ID.
        /// </summary>
        public int UserId { get; set; }

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

        /// <summary>
        /// Determines the type of context in the synchronization system. Relevant only for old Yodlee users.
        /// </summary>
        public bool IsPrivateUser { get; set; }
    }
}
