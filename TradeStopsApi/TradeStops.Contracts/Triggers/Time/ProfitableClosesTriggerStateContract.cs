using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ProfitableClosesTriggerStateContract : BaseTriggerStateContract
    {
        public ProfitableClosesTriggerStateContract()
            : base(TriggerTypes.ProfitableCloses)
        {
        }

        public PriceTypes PriceType { get; set; }
        public decimal CurrentValue { get; set; }
    }
}
