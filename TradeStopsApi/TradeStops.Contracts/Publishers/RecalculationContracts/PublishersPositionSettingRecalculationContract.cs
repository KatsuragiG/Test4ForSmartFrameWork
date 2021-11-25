using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Position settings data for calculations. 
    /// </summary>
    public class PublishersPositionSettingRecalculationContract
    {
        /// <summary>
        /// Stop Priority type.
        /// </summary>
        public PublishersStopPriorityTypes? StopPriority { get; set; }

        /// <summary>
        /// Trailing stop level (%).
        /// </summary>
        public decimal? TrailingStop { get; set; }

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
