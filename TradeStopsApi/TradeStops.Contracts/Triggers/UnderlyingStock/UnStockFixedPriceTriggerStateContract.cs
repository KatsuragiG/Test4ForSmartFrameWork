using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UnStockFixedPriceTriggerStateContract : BaseTriggerStateContract
    {
        public UnStockFixedPriceTriggerStateContract()
            : base(TriggerTypes.UnStockFixedPrice)
        {
        }

        public PriceTypes PriceType { get; set; }
        public decimal LatestPrice { get; set; }
        public string Currency { get; set; }
        public bool UseIntraday { get; set; }
    }
}
