using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class DollarTimeValueTriggerStateContract : BaseTriggerStateContract
    {
        public DollarTimeValueTriggerStateContract()
            : base(TriggerTypes.DollarTimeValue)
        {
        }

        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
    }
}
