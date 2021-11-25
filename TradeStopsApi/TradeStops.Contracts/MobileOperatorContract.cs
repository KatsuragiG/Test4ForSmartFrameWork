namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class MobileOperatorContract
    {
        /// <summary>
        /// Mobile operator Id.
        /// </summary>
        public int MobileOperatorId { get; set; }

        /// <summary>
        /// Mobile operator name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Postfix for sending message to mobile operator.
        /// </summary>
        public string Postfix { get; set; }
    }
}
