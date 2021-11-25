using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RecentEventsContract
    {
        public List<SystemEventContract> SystemEvents { get; set; }

        public List<CorporateActionEventContract> CorporateActions { get; set; }
    }
}
