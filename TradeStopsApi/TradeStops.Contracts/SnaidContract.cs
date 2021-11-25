namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SnaidContract
    {
        public SnaidContract()
        {
        }

        public SnaidContract(string snaid)
        {
            Snaid = snaid;
        }

        public string Snaid { get; set; }
    }
}
