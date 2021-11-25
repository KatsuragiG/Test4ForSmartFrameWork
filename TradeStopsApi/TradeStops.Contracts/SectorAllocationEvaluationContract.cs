using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SectorAllocationEvaluationContract
    {
        public SectorAllocationEvaluation Evaluation { get; set; }

        public List<string> SectorsToConsider { get; set; }

        //public class Sector
        //{
        //    public string SectorName { get; set; }
        //}
    }
}
