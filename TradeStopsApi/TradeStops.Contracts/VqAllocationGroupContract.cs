using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class VqAllocationGroupContract
    {
        /// <summary>
        /// VQ Allocation group name.
        /// </summary>
        public VqAllocationGroups Group { get; set; }

        /// <summary>
        /// Percent of portfolio under the same symbol.
        /// </summary>
        public decimal PercentOfPortfolio { get; set; }

        /// <summary>
        /// VqAllocation Symbol Contract values.
        /// </summary>
        public List<VqAllocationSymbolContract> Symbols { get; set; }
    }
}