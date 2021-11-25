using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Asset Allocation for newsletter portfolios
    /// </summary>
    public class NewslettersAssetAllocationContract
    {
        /// <summary>
        /// Values of the asset allocations by industry.
        /// </summary>
        public List<NewslettersAssetAllocationGroupContract> AllocationsByIndustry { get; set; }

        /// <summary>
        /// Values of the asset allocations by sector.
        /// </summary>
        public List<NewslettersAssetAllocationGroupContract> AllocationsBySector { get; set; }
    }
}
