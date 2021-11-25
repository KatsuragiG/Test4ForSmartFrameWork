using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Asset Allocation
    /// </summary>
    public class AssetAllocationContract
    {
        /// <summary>
        /// Portfolio IDs.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// PortfolioTypes value in the in TradeStops API.
        /// </summary>
        public PortfolioTypes? PortfolioType { get; set; }

        /// <summary>
        /// Weight of the cash asset in the portfolio value - value in [0..1] interval
        /// </summary>
        public decimal CashWeight { get; set; }

        /// <summary>
        /// Values of the asset allocations by industry.
        /// </summary>
        public List<AssetAllocationPointContract> AllocationsByIndustry { get; set; }

        /// <summary>
        /// Values of the asset allocations by sectors.
        /// </summary>
        public List<AssetAllocationPointContract> AllocationsBySector { get; set; }

        /// <summary>
        /// Values of the asset allocations by sectors including portfolios cash in the analysis.
        /// </summary>
        public List<AssetAllocationPointContract> AllocationsByIndustryWithoutCash { get; set; }

        /// <summary>
        /// Values of the asset allocations by sectors without including portfolios cash in the analysis.
        /// </summary>
        public List<AssetAllocationPointContract> AllocationsBySectorWithoutCash { get; set; }
    }
}
