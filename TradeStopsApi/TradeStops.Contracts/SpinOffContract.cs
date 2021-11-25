using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SpinOffContract
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
        /// Symbol ID of new stock.
        /// </summary>
        public int DistributedSymbolId { get; set; }

        /// <summary>
        /// Number of new stock shares as a spin off result
        /// </summary>
        public float NumberOfGivenShares { get; set; }

        /// <summary>
        /// Date when the spin off will be paid.
        /// </summary>
        public DateTime PayDate { get; set; }
    }
}
