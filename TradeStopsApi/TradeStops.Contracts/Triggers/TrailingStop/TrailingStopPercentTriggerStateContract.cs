using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class TrailingStopPercentTriggerStateContract : BaseTriggerStateContract
    {
        public TrailingStopPercentTriggerStateContract()
            : base(TriggerTypes.TrailingStopPercent)
        {
        }

        public decimal CurrentValue { get; set; }
        public TradeTypes TradeType { get; set; }
        public PriceTypes PriceType { get; set; }
        public decimal ExtremumPrice { get; set; }
        public DateTime ExtremumDate { get; set; }
        public decimal StopPrice { get; set; }
        public string Currency { get; set; }
        public bool UseIntraday { get; set; }
    }
}
