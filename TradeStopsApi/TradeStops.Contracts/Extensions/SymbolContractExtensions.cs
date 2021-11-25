using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts.Extensions
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public static class SymbolContractExtensions
    {
        public static bool IsStock(this SymbolContract symbol)
        {
            return symbol.Option == null;
        }

        public static bool IsOption(this SymbolContract symbol)
        {
            return symbol.Option != null;
        }
    }
}
