using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class LinePointContract
    {
        /// <summary>
        /// Date of the chart line item.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Item value of the chart line item.
        /// </summary>
        public decimal? Value { get; set; }
    }
}
