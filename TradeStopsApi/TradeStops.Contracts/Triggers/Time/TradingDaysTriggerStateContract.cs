using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class TradingDaysTriggerStateContract : BaseTriggerStateContract
    {
        public TradingDaysTriggerStateContract()
            : base(TriggerTypes.TradingDays)
        {
        }

        public decimal CurrentValue { get; set; }
    }
}
