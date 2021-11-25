using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PortfolioForSelectContract
    {
        public int PortfolioId { get; set; }

        public string Name { get; set; }

        public List<PositionForSelectContract> PositionsForSelectContract { get; set; }

        public bool IsSynchronized { get; set; }

        public PortfolioTypes PortfolioType { get; set; }
    }
}