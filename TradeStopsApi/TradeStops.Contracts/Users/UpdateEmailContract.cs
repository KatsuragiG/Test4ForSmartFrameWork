using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UpdateEmailContract
    {
        public UpdateEmailContract()
        {
        }

        public UpdateEmailContract(string newEmail)
        {
            NewEmail = newEmail;
        }

        public string NewEmail { get; set; }
    }
}