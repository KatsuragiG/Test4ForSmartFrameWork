using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PercentOfAverageVolumeTriggerStateContract : BaseTriggerStateContract
    {
        public PercentOfAverageVolumeTriggerStateContract()
            : base(TriggerTypes.PercentOfAverageVolume)
        {
        }

        public int Period { get; set; }
        public PeriodTypes PeriodType { get; set; }
        public decimal CurrentValue { get; set; }
        public TriggerOperationTypes OperationType { get; set; }
    }
}
