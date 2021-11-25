using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PortfolioAssessmentContract
    {
        public NotablePositionsEvaluationContract NotablePositionsEvaluation { get; set; }
        public SsiDistributionEvaluation SsiDistributionEvaluation { get; set; }
        public NumberOfHoldingsEvaluation NumberOfHoldingsEvaluation { get; set; }
        public VqAllocationEvaluation VqAllocationEvaluation { get; set; }
        public DiversificationByVqEvaluation DiversificationByVqEvaluation { get; set; }
        public SectorAllocationEvaluationContract SectorAllocationEvaluation { get; set; }
    }
}
