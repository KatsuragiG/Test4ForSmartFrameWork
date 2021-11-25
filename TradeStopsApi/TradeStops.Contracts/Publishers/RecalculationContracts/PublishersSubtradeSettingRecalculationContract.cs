using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Subtrade settings data for calculations.
    /// </summary>
    public class PublishersSubtradeSettingRecalculationContract
    {
        /// <summary>
        /// Stop Priority type.
        /// </summary>
        public PublishersStopPriorityTypes? StopPriority { get; set; }

        /// <summary>
        /// Defines whether to use adjusted prices for extremums.
        /// </summary>
        public bool IsExtremumAdjustedPrice { get; set; }

        /// <summary>
        /// Trailing stop level (%).
        /// </summary>
        public decimal? TrailingStop { get; set; }

        /// <summary>
        /// Smart Trailing stop level (%).
        /// </summary>
        public decimal? SmartStop { get; set; }

        /// <summary>
        /// TS Minus Dividend level (%).
        /// </summary>
        public decimal? TSMinusDividend { get; set; }

        /// <summary>
        /// Hard stop level (%).
        /// </summary>
        public decimal? HardStop { get; set; }

        /// <summary>
        /// Close Above stop level.
        /// </summary>
        public decimal? CloseAboveStop { get; set; }

        /// <summary>
        /// Close Below stop level.
        /// </summary>
        public decimal? CloseBelowStop { get; set; }
    }
}
