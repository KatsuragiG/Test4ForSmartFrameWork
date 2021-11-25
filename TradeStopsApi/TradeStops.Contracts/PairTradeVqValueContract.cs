using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pair Trade VQ value
    /// </summary>
    public class PairTradeVqValueContract
    {
        /// <summary>
        /// Position ID for vq value
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Date and time when vq value was calculated
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Vq value
        /// </summary>
        public decimal Value { get; set; }
    }
}
