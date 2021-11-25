using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetChartItemsForPositionContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// List of chart item types to load
        /// </summary>
        public List<ChartItemTypes> ChartItemsToLoad { get; set; }
    }
}
