namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PositionTagContract
    {
        /// <summary>
        /// Primary key of the position and tag connection.
        /// </summary>
        public int UserPositionTagId { get; set; } // we use this field for sorting tags on client-side. this order of tags was requested by customers

        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Tag ID.
        /// </summary>
        public int UserTagId { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string Tag { get; set; }
    }
}
