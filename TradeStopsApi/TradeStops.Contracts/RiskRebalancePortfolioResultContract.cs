using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalancePortfolioResultContract
    {
        /// <summary>
        /// Values of the position  risk rebalance results
        /// </summary>
        public IList<RiskRebalanceResultPositionContract> RebalancedPositions { get; set; }

        /// <summary>
        /// Values of the risk rebalance results for excluded positions. Excluded positions are not taken into account for PortfolioVq calculation
        /// </summary>
        public IList<RiskRebalanceResultExcludedPositionContract> ExcludedPositions { get; set; }

        /// <summary>
        /// Value of the risk per position. Locked positions are not included into calculations of RiskPerPosition.
        /// </summary>
        public decimal RiskPerPosition { get; set; }

        /// <summary>
        /// Rebalanced value of the portfolio cash.
        /// </summary>
        public decimal RebalancedPortfolioCash { get; set; }

        /// <summary>
        /// Percent value of the risk per position.
        /// </summary>
        public decimal RiskPerPositionsPercent { get; set; }

        /// <summary>
        /// Values of the Original Portfolio Vq Allocation. 
        /// </summary>
        public VqAllocationContract OriginalPortfolioVqAllocation { get; set; }

        /// <summary>
        /// Values of the Rebalanced Portfolio Vq Allocation.
        /// </summary>
        public VqAllocationContract RebalancedPortfolioVqAllocation { get; set; }

        /// <summary>
        /// All calculations will be performed in this currency in case positions were provided  in multiple currencies.
        /// </summary>
        public int DefaultCurrencyId { get; set; }
    }
}
