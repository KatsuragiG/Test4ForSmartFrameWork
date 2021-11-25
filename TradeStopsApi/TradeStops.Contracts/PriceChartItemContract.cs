using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PriceChartItemContract
    {
        /// <summary>
        /// Item values of the chart line.
        /// </summary>
        public List<LinePointContract> Line { get; set; }

        /// <summary>
        /// Flag values of chart items.
        /// </summary>
        public List<ChartFlagContract> Flags { get; set; }

        /// <summary>
        /// Chat item type.
        /// </summary>
        public ChartItemTypes ItemType { get; set; }
    }
}