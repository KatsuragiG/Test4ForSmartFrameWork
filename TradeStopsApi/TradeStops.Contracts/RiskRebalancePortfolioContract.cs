using System.Collections.Generic;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalancePortfolioContract // todo: rename to ...InputContract
    {
        /// <summary>
        /// The method returns the rebalanced portfolio information.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Value of the additional cash to be invested.
        /// </summary>
        public decimal CustomPortfolioCash { get; set; }

        /// <summary>
        ///  Determines if dividends will be included to the total value for rebalancing calculations.
        /// </summary>
        public bool IncludeDividends { get; set; }

        /// <summary>
        /// Ids of the positions to be excluded from rebalancing calculations. Excluded positions are not taken for PortfolioVq calculation.
        /// </summary>
        public IList<int> ExcludedPositionIds { get; set; }

        /// <summary>
        /// Ids of the positions to include to rebalancing calculations without the adjustment in the result. Locked positions are taken for PortfolioVq calculation. Are not used for calculations of RiskPerPosition
        /// </summary>
        public IList<int> LockedPositionsIds { get; set; }

        /// <summary>
        /// Reallocate funds from positions that are Stopped Out according to the SSI.
        /// </summary>
        public bool RellocateStoppedOutPositions { get; set; }

        /// <summary>
        /// Values for added stocks (The values are always required, if at least one stock  is  added).
        /// </summary>
        public List<RiskRebalancerAddedStockContract> AddedStocks { get; set; }

        /// <summary>
        /// Determines if the fractional number of shares is allowed in the result.
        /// </summary>
        public bool AllowFractionalShares { get; set; }

        /// <summary>
        /// (Optional) Id of currency which will be used for calculation
        /// </summary>
        public Optional<int> DefaultCurrencyId { get; set; }
    }
}
