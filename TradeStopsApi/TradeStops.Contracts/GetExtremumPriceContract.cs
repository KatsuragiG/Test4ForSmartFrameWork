using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class GetExtremumPriceContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Defines the start date of the range.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Defines the end date of the range.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Defines whether maximum price will be searched using prices adjusted by dividends or not.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        /// (optional) Defines the price type to search.
        /// </summary>
        public PriceTypes? PriceType { get; set; }
    }
}
