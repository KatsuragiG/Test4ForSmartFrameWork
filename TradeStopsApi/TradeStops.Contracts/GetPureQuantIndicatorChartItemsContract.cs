using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetPureQuantIndicatorChartItemsContract
    {
        /// <summary>
        /// Market Outlook ID
        /// </summary>
        public int MarketOutlookId { get; set; }

        /// <summary>
        /// Pure Quant threshold value
        /// </summary>
        public decimal Threshold { get; set; }

        /// <summary>
        /// Min date of first chart item
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Max date of last chart item
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// List of chart item types to load
        /// </summary>
        public List<ChartItemTypes> ChartItemsToLoad { get; set; }
    }
}
