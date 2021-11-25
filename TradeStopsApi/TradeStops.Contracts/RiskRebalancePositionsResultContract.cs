using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalancePositionsResultContract
    {
        public int DefaultCurrencyId { get; set; }

        public decimal OriginalCash { get; set; }

        public decimal RebalancedCash { get; set; }

        public bool IncludeDividends { get; set; }

        public decimal RiskPerPosition { get; set; }

        public decimal RiskPerPositionsPercent { get; set; }

        public List<RiskRebalanceResultPositionContract> Positions { get; set; }
    }
}
