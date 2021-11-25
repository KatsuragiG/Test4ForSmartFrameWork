namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class CreatePortfolioAnalyzerUserContract
    {
        public string Snaid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
