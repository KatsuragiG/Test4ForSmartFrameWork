using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetChartItemsForSymbolContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date when the position was purchased.
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Determines if the prices on a chart adjusted by dividends.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        /// Position Trade type. 
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Trailing stops percent value used for creating VQ Trailing Stop line.
        /// </summary>
        public decimal TsPercent { get; set; }

        /// <summary>
        /// Start date of chart lines.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date of chart lines.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// List of chart item types to load
        /// </summary>
        public List<ChartItemTypes> ChartItemsToLoad { get; set; }
    }
}
