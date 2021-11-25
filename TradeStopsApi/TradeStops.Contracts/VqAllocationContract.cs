using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class VqAllocationContract
    {
        /// <summary>
        /// Volatility Quotient Allocation Group Contract values.
        /// </summary>
        public List<VqAllocationGroupContract> Groups { get; set; }

        /// <summary>
        /// Measures the volatility of the whole portfolio
        /// </summary>
        public decimal? PortfolioVq { get; set; }
    }
}