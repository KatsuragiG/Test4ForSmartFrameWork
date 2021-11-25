using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class SavePortfolioAnalyzerSummaryTaskContract
    {
        public string Url { get; set; }
    }
}
