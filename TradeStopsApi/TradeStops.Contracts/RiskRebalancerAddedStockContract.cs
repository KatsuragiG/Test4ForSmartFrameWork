namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalancerAddedStockContract
    {
        /// <summary>
        /// Symbol ID of the added stock.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Determines if the added stock is locked and  included to rebalancing calculations without the adjustment in the result.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Value of the asset weight by position of the portfolio value according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedWeight { get; set; }

        /// <summary>
        /// Percent of the asset weight by position of the portfolio value according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedWeightPercent { get; set; }

        /// <summary>
        /// Risk value of the individual position compared to the value of the overall portfolio according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedRisk { get; set; }

        /// <summary>
        /// Number of shares of the added stock according to rebalancing calculations.
        /// </summary>
        public decimal UpdatedShares { get; set; }

        /// <summary>
        /// Difference between current shares in the portfolio and adjusted shares number.
        /// </summary>
        public decimal SharesDifference { get; set; }

        /// <summary>
        /// Amount value to be invested according to rebalancing calculations.
        /// </summary>
        public decimal AmtInvested { get; set; }
    }
}
