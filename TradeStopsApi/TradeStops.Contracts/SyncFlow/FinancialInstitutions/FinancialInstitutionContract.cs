using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Financial institution is something that contain information about broker;
    /// Used in portfolio synchronization
    /// </summary>
    public class FinancialInstitutionContract
    {
        /// <summary>
        /// Broker ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        /// Broker name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Internal broker ID in the Vendor's system.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Defines that broker has the multi-factor authentication.
        /// </summary>
        public bool IsMfa { get; set; }

        /// <summary>
        /// Link to the homepage of the brokerage website.
        /// </summary>
        public string HomePage { get; set; }

        /// <summary>
        /// Link to the login page of the brokerage website.
        /// </summary>
        public string LoginUrl { get; set; }

        /// <summary>
        /// Defines that the current user has access for importing portfolios from the brokerage website.
        /// </summary>
        public bool UserHasAccess { get; set; }

        /// <summary>
        /// Defines that a broker migrated to another one and is no longer supported.
        /// </summary>
        public bool WasMigrated { get; set; }

        /// <summary>
        /// Synchronization vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }
    }
}
