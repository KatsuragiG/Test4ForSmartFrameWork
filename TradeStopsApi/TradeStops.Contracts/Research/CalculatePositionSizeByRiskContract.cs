using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to calculate position size by risk amount
    /// </summary>
    public class CalculatePositionSizeByRiskContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Position entry price.
        /// </summary>
        public decimal EntryPrice { get; set; }

        /// <summary>
        /// Position Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Amount of risk
        /// </summary>
        public decimal RiskAmount { get; set; }

        /// <summary>
        ///  Stop loss strategy type
        /// </summary>
        public StopTypes StopType { get; set; }

        /// <summary>
        /// (optional) Threshold value of a stop loss strategy. Available only for the TsPercent and FixedStopPrice strategies.
        /// </summary>
        public decimal? ThresholdValue { get; set; }

        /// <summary>
        /// Determines if the fractional number of shares is allowed in the result.
        /// </summary>
        public bool AllowFractionalShares { get; set; }
    }
}
