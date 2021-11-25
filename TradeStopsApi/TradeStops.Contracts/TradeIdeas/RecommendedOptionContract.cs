namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RecommendedOptionContract
    {
        public SymbolContract Symbol { get; set; }

        public PriceContract LatestPrice { get; set; }

        public CurrencyContract Currency { get; set; }

        public PriceContract TriggerPrice { get; set; }
    }
}
