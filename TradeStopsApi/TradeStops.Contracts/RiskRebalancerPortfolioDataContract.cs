using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalancerPortfolioDataContract
    {
        /// <summary>
        /// Requested portfolio IDs.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Requested portfolios don't have any positions.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Portfolio default currency ID.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Requested portfolio IDs.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Positions value without dividends value included.
        /// </summary>
        public decimal PositionsValueWithoutDividends { get; set; }

        /// <summary>
        /// Total portfolios dividends value.
        /// </summary>
        public decimal Dividends { get; set; }

        /// <summary>
        /// Total portfolios cash.
        /// </summary>
        public decimal Cash { get; set; }
    }
}