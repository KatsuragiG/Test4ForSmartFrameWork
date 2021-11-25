namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class CreateSystemAlertTriggeredEventContract
    {
        public int UserId { get; set; }
        public int PortfolioId { get; set; }
        public int PositionId { get; set; }
        public TriggerFieldsContract TriggerFields { get; set; }
        public TriggerStateFieldsContract TriggerStateFields { get; set; }
    }
}
