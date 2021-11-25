using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class SynchronizeByTradeSmithUserGuidContract
    {
        public Guid TradeSmithUserGuid { get; set; }

        public Products ProductId { get; set; }
    }
}
