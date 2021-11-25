using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ProductSubscriptionPubCodeContract
    {
        public int ProductSubscriptionPubCodeId { get; set; }

        public ProductSubscriptions ProductSubscription { get; set; }

        public string PubCode { get; set; }

        public SyncronizationSources SyncSource { get; set; }
    }
}
