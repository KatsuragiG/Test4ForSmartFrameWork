namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class StrikePriceContract
    {
        /// <summary>
        /// Option strike price.
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// Option type (P for Put and C for Call types).
        /// </summary>
        public string Type { get; set; }
    }
}
