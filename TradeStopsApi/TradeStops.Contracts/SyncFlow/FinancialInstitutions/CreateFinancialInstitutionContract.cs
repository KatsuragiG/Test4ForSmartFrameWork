using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create financial institution
    /// </summary>
    public class CreateFinancialInstitutionContract
    {
        /// <summary>
        /// Broker name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Internal broker ID in the Vendor's system.
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// Multifactor authentication type.
        /// </summary>
        public FinancialInstitutionMfaTypes? MfaType { get; set; }

        /// <summary>
        /// Link to the login page of the brokerage website.
        /// </summary>
        public string LoginUrl { get; set; }

        /// <summary>
        /// Link to the homepage of the brokerage website.
        /// </summary>
        public string HomePage { get; set; }

        /// <summary>
        /// Notes for brokerage.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Synchronization vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        /// Defines if financial institution is available for all users.
        /// </summary>
        public bool Visible { get; set; }
    }
}
