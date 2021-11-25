using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class CheckNewslettersCredentialsResultContract
    {
        public CheckNewslettersCredentialsResults Result { get; set; }
    }
}
