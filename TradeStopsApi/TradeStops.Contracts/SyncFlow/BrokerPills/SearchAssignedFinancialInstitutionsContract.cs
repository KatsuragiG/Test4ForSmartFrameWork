namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class SearchAssignedFinancialInstitutionsContract : GridSearchContract
    {
        public int BrokerPillId { get; set; }
    }
}
