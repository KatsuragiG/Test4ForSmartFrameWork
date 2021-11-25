using System;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class TrackPortfolioAnalyzerVisitContract
    {
        public string Snaid { get; set; }   

        public string PageUrl { get; set; }

        public string ActionTaken { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid VisitId { get; set; }
    }
}
