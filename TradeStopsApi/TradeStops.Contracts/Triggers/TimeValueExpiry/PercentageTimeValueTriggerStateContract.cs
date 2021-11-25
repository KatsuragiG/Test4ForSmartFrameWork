using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PercentageTimeValueTriggerStateContract : BaseTriggerStateContract
    {
        public PercentageTimeValueTriggerStateContract()
            : base(TriggerTypes.PercentageTimeValue)
        {
        }

        public decimal LatestTimeValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal InitialTimeValue { get; set; }
        public string Currency { get; set; }
    }
}
