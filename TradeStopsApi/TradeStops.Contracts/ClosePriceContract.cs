using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Close Price contract.
    /// Useful for calculations where you need only close price fields to decrease response size.
    /// </summary>
    public class ClosePriceContract
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
        /// Closing price for the given Trade Date. This value is not adjusted by historical corporate actions.
        /// </summary>
        public decimal TradeClose { get; set; }

        /// <summary>
        /// Multiplier represents coefficients of splits, stock distributions and spin offs. Can be used to adjust price by these 3 corporate actions.
        /// TradeStops always adjusts all prices by this multiplier.
        /// </summary>
        public decimal SplitMultiplier { get; set; }

        /// <summary>
        /// Multiplier represents coefficients of paid dividends.Can be used to adjust price by dividends.
        /// </summary>
        public decimal DividendMultiplier { get; set; }
    }
}
