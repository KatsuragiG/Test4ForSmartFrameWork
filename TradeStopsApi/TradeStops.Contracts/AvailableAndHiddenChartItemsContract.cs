using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class AvailableAndHiddenChartItemsContract
    {
        public List<ChartItemTypes> AvalableChartItems { get; set; }

        public List<ChartItemTypes> HiddenChartItems { get; set; }
    }
}
