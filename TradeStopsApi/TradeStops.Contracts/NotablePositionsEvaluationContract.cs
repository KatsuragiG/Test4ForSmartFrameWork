using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class NotablePositionsEvaluationContract
    {
        public NotablePositionsEvaluation Evaluation { get; set; }

        public List<SymbolContract> Symbols { get; set; }

        //public List<PositionContract> Positions { get; set; } // todo: what if user has long and short apple positions? comparing just by symbolId won't be enough in this case
    }
}
