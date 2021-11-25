namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class RiskRebalanceResultExcludedPositionContract
    {
        /// <summary>
        /// ID of the excluded position.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Symbol of of the excluded position.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Rebalanced position info.
        /// </summary>
        public RiskRebalanceResultPositionContract PositionInfo { get; set; }
    }
}
