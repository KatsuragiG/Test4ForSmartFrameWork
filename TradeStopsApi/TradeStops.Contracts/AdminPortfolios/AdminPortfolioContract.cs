using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio in the admin area for the user.
    /// </summary>
    public class AdminPortfolioContract
    {
        /// <summary>
        /// Unique portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// Portfolio sync type in the admin area.
        /// </summary>
        public PortfolioSyncTypes PortfolioSyncType { get; set; }

        /// <summary>
        /// Portfolio status in the admin area.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// Synchronization vendor type.
        /// </summary>
        public VendorTypes? VendorType { get; set; }

        /// <summary>
        /// Unique identifier of the user account in the vendor system.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        /// Unique identifier for the synchronized portfolio in the vendor system.
        /// </summary>
        public string VendorPortfolioId { get; set; }

        /// <summary>
        /// Broker name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        /// Number of positions in this portfolio.
        /// </summary>
        public int PositionsCount { get; set; }
    }
}
