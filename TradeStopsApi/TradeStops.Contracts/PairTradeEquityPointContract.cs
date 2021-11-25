using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PairTradeEquityPointContract
    {
        public DateTime TradeDate { get; set; }

        public decimal AbsoluteEquity { get; set; }
    }
}
