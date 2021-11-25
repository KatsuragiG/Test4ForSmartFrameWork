using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SplitContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Split issue date
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Coefficient of old shares.
        /// </summary>
        public decimal OldShares { get; set; }

        /// <summary>
        /// Coefficient of new shares.
        /// </summary>
        public decimal NewShares { get; set; }
    }
}
