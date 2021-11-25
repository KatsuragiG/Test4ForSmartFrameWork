using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with VQ (Volaitlity Quotient) value for corresponding symbol
    /// </summary>
    public class VqValueContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date of VQ recalculation
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// VQ Value in [5..95] range
        /// </summary>
        public decimal VqValue { get; set; }
    }
}
