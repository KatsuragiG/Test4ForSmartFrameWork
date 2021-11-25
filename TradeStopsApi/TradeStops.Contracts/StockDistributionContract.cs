using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class StockDistributionContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// If the user holds shares before that date, the spin off will be paid.
        /// </summary>
        public DateTime ExDate { get; set; }

        /// <summary>
        /// Number of additional shares as a distribution result.
        /// </summary>
        public float NumberOfGivenShares { get; set; }

        /// <summary>
        /// Date when the distribution will be paid.
        /// </summary>
        public DateTime PayDate { get; set; }
    }
}
