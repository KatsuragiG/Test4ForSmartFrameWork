using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts.Extensions
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public static class PortfolioContractExtensions
    {
        public static bool IsSynchronized(this PortfolioContract contract)
        {
            return contract.ImportedPortfolio != null;
        }
    }
}
