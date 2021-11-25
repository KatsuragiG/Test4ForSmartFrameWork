namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class TagContract
    {
        /// <summary>
        /// Tag ID.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Number value of positions to which the tag is assigned.
        /// </summary>
        public int PositionsCount { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string Tag { get; set; }
    }
}
