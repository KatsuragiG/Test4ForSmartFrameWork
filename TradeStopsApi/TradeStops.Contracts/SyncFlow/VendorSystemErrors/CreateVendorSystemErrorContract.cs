using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create vendor system error
    /// </summary>
    public class CreateVendorSystemErrorContract
    {
        /// <summary>
        ///  (optional) Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        ///  (optional) Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  User email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///  TradeSmith user ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }
    }
}
