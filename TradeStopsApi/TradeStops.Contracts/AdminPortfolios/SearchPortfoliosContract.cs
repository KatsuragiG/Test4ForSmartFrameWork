using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Filters for admin portfolios grid.
    /// </summary>
    public class SearchPortfoliosContract
    {
        /// <summary>
        /// (optional) If true then return only delisted portfolios, if false - only not delisted.
        /// </summary>
        public bool? IsFilteredByDelistedPortfolios { get; set; }

        /// <summary>
        /// (optional) Portfolio sync type.
        /// </summary>
        public PortfolioSyncTypes? PortfolioSyncType { get; set; }

        /// <summary>
        /// (optional) Synchronization vendor type.
        /// </summary>
        public VendorTypes? VendorType { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///  Search key word.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        ///  Search by field.
        /// </summary>
        public SearchPortfoliosSearchByFields? SearchByField { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public SearchPortfoliosOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
