using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class BreakoutTriggerStateContract : BaseTriggerStateContract
    {
        public BreakoutTriggerStateContract()
            : base(TriggerTypes.Breakout)
        {
        }

        public PriceTypes PriceType { get; set; }
        public int Period { get; set; }
        public PeriodTypes PeriodType { get; set; }
        public decimal? ExtremumPrice { get; set; } // one case, when extremum price can be null - incomplete yodlee option
        public decimal LatestPrice { get; set; }
        public TriggerOperationTypes OperationType { get; set; }
        public string Currency { get; set; }
        public bool UseIntraday { get; set; }
    }
}
