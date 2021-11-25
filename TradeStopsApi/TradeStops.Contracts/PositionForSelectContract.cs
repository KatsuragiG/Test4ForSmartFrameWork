using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PositionForSelectContract
    {
        public int PositionId { get; set; }

        public int CurrencyId { get; set; }

        public string Symbol { get; set; }

        public PositionStatusTypes StatusType { get; set; }

        public PositionTypes PositionType { get; set; }
    }
}