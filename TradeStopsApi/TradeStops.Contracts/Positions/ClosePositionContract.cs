using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to close position
    /// </summary>
    public class ClosePositionContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Position close date.
        /// </summary>
        public DateTime CloseDate { get; set; }

        /// <summary>
        ///  Position close fee.
        /// </summary>
        public decimal CloseFee { get; set; }

        /// <summary>
        /// (optional) Position close price.
        /// </summary>
        public decimal? ClosePrice { get; set; } // currently we allow to close pairtrade without close price, it is better to calculate close price and show it in popup

        /// <summary>
        /// (optional) Number of the shares to close.
        /// </summary>
        public decimal? SharesToSell { get; set; }
    }
}
