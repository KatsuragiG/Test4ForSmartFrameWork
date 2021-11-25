using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class TargetTriggerStateContract : BaseTriggerStateContract
    {
        public TargetTriggerStateContract()
            : base(TriggerTypes.Target)
        {
        }

        public decimal ThresholdValue { get; set; }
        public TargetColumnNames TargetColumnName { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
    }
}
