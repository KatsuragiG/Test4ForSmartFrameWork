using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Price
    /// </summary>
    public class PriceContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Opening  price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeOpen { get; set; }

        /// <summary>
        /// Highest price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeHigh { get; set; }

        /// <summary>
        /// Lowest price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeLow { get; set; }

        /// <summary>
        /// Closing price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeClose { get; set; }

        /// <summary>
        /// (optional) Trade volume.
        /// </summary>
        public long? TradeVolume { get; set; }

        /// <summary>
        /// Multiplier represents coefficients of splits, stock distributions and spin offs. Can be used to adjust price by these 3 corporate actions.
        /// TradeStops always adjusts all prices by this multiplier.
        /// </summary>
        public decimal SplitMultiplier { get; set; }

        /// <summary>
        /// Multiplier represents coefficients of paid dividends.Can be used to adjust price by dividends.
        /// </summary>
        public decimal DividendMultiplier { get; set; }

        /// <summary>
        /// Volume Weighted Average Price at pay date.
        /// </summary>
        public decimal VolumeWeightedAveragePrice { get; set; }
    }
}
