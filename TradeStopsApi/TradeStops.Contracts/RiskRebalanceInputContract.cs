using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalanceInputContract
    {
        /// <summary>
        /// All calculations will be performed in this currency in case positions were provided  in multiple currencies.
        /// </summary>
        public int DefaultCurrencyId { get; set; }

        /// <summary>
        /// Additional cash to be invested. Original portfolio size will be calculated based on positions value and additional cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// Determines if dividends will be included to the total value for rebalancing calculations.
        /// </summary>
        public bool IncludeDividends { get; set; }

        /// <summary>
        /// Total amount you would invest into all of your new positions. If this field is provided, original portfolios size will be takes as this value.
        /// </summary>
        public decimal CustomInvestmentAmount { get; set; }

        /// <summary>
        /// Reallocate funds from positions that are Stopped Out according to the SSI.
        /// </summary>
        public bool RellocateStoppedOutPositions { get; set; }

        /// <summary>
        /// Values of positions included in the risk rebalancer list.
        /// </summary>
        public List<RiskRebalanceInputPositionContract> Positions { get; set; }

        /// <summary>
        /// Values for added stocks (The values are always required , if at least one stock  is  added ).
        /// </summary>
        public List<RiskRebalancerAddedStockContract> AddedStocks { get; set; }

        /// <summary>
        /// Determines if the fractional number of shares is allowed in the result.
        /// </summary>
        public bool AllowFractionalShares { get; set; }

        // public decimal ExcludedPositionsCash { get; set; }
    }
}
