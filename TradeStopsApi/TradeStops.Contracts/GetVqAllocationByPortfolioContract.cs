using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetVqAllocationByPortfolioContract
    {
        /// <summary>
        /// Portfolio IDs for analyzing.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Include portfolios cash in the analysis.
        /// </summary>
        public bool IncludeCash { get; set; }
    }
}
