using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetRiskRebalancerPortfoliosDataContract
    {
        public IList<RiskRebalancerPortfolioDataContract> Portfolios { get; set; }

        public int DefaultCurrencyId { get; set; }

        public string DefaultCurrencySymbol { get; set; }
    }
}
