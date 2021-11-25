using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class VqAllocationSymbolContract
    {
        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Symbol Volatility Quotient percent.
        /// </summary>
        public decimal? VqPercent { get; set; }

        /// <summary>
        /// Percent of portfolio under the same symbol.
        /// </summary>
        public decimal PercentOfPortfolio { get; set; }

        /// <summary>
        /// All position Ids related to this symbol.
        /// </summary>
        public List<int> RelatedPositionIds { get; set; }
    }
}