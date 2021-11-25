using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class MovingAveragePriceTriggerStateContract : BaseTriggerStateContract
    {
        public MovingAveragePriceTriggerStateContract()
            : base(TriggerTypes.MovingAveragePrice)
        {
        }

        public PriceTypes PriceType { get; set; }
        public int Period { get; set; }
        public PeriodTypes PeriodType { get; set; }
        public decimal LatestPrice { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal AveragePrice { get; set; }
        public string Currency { get; set; }
    }
}
