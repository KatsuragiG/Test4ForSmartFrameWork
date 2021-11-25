using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class MovingAverageCrossesTriggerStateContract : BaseTriggerStateContract
    {
        public MovingAverageCrossesTriggerStateContract()
            : base(TriggerTypes.MovingAverageCrosses)
        {
        }

        public int Period { get; set; }
        public PeriodTypes PeriodType { get; set; }
        public int Period2 { get; set; }
        public PeriodTypes PeriodType2 { get; set; }
        public PriceTypes PriceType { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal AveragePrice2 { get; set; }
        public decimal CurrentValue { get; set; }
        public string Currency { get; set; }
    }
}
