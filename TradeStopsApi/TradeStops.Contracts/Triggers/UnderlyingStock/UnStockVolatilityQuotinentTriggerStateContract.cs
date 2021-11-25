using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UnStockVolatilityQuotinentTriggerStateContract : BaseTriggerStateContract
    {
        public UnStockVolatilityQuotinentTriggerStateContract()
            : base(TriggerTypes.UnStockVolatilityQuotinent)
        {
        }

        public TradeTypes TradeType { get; set; }
        public decimal CurrentValue { get; set; }
        public PriceTypes PriceType { get; set; }
        public decimal ExtremumPrice { get; set; }
        public DateTime ExtremumDate { get; set; }
        public decimal StopPrice { get; set; }
        public string Currency { get; set; }
    }
}
