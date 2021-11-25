namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class AgoraCustomerNumberContract
    {
        public AgoraCustomerNumberContract()
        {
        }

        public AgoraCustomerNumberContract(string acn)
        {
            AgoraCustomerNumber = acn;
        }

        public string AgoraCustomerNumber { get; set; }
    }
}
