using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class BaseTokenPayloadContract
    {
        public Guid JwtId { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpirationAt { get; set; }
    }
}
