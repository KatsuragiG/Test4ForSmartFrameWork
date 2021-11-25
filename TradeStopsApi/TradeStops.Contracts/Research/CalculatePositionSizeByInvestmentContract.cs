using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to calculate position size by investment amount
    /// </summary>
    public class CalculatePositionSizeByInvestmentContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Position investment amount.
        /// </summary>
        public decimal InvestmentAmount { get; set; }

        /// <summary>
        /// Specify how stop price should be calculated. Only VqPercent and TsPercent types are supported.
        /// </summary>
        public StopTypes StopType { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// (optional) Threshold value of a stop loss strategy. Available only for the TsPercent.
        /// </summary>
        public decimal? ThresholdValue { get; set; }

        /// <summary>
        /// (optional) Entry price, used in the calculations of the Shares/Contracts to Buy/Sell
        /// </summary>
        public decimal? EntryPrice { get; set; }
    }
}
