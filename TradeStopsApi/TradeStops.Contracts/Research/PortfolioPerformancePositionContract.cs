using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Position data to calculate performance line
    /// </summary>
    public class PortfolioPerformancePositionContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Purchase (entry) date
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Purchase (entry) price
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// (optional) Close date
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// (optional) Close price
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// (optional) Number of shares, adjusted
        /// </summary>
        public decimal SharesAdjusted { get; set; }

        /// <summary>
        /// Trade type
        /// </summary>
        public TradeTypes TradeType { get; set; }    

        /// <summary>
        /// Status type
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }
    }
}
