using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Pair Trade subtrade
    /// </summary>
    public class CreatePairTradeSubtradeContract
    {
        /// <summary>
        ///  Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Cash asset weight of the Pair Trade position value.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; } 
    }
}
