using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UserTransactionContract
    {
        public int UserTransactionId { get; set; }

        public decimal Amount { get; set; }

        public DateTime EventDate { get; set; }

        public string Type { get; set; }
    }
}
