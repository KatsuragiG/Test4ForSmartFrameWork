using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit PositionTrigger
    /// </summary>
    public class EditPtPositionTriggerContract
    {
        /// <summary>
        /// The Position Trigger start date is used as a start point for the trigger calculation and could be specified by the user. If the field is skipped, the Position Trigger start date will be taken from position Entry Date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// ThresholdValue
        /// </summary>
        public decimal? ThresholdValue { get; set; }

        /// <summary>
        /// Type of the price for setting up a Position Trigger.
        /// </summary>
        public PriceTypes? PriceType { get; set; }

        /// <summary>
        /// Type of the Position Trigger (TrailingStopPercent or VolatilityQuotinent).
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Type of the operation. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes? OperationType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// Type of the period of time.
        /// </summary>
        public PeriodTypes? PeriodType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time when the moving average has to be equal to the specified percent value to satisfy the trigger operation type.
        /// </summary>
        public int? Period2 { get; set; }

        /// <summary>
        /// Type of the trigger Period2 .
        /// </summary>
        public PeriodTypes? PeriodType2 { get; set; }

        /// <summary>
        /// Type of the  fundamental data.
        /// </summary>
        public TargetColumnNames? TargetColumnName { get; set; }

        /// <summary>
        /// Date when the alert will be triggered.
        /// </summary>
        public DateTime? ThresholdDate { get; set; }

        /// <summary>
        /// Price value of the Underlying Stock.
        /// </summary>
        public decimal? StockPurchasePrice { get; set; }

        /// <summary>
        /// Trigger alert for the Down Trend state.
        /// </summary>
        public bool? TrackDownTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Entry Signal state.
        /// </summary>
        public bool? TrackEntrySignal { get; set; }

        /// <summary>
        /// Trigger alert for the Side Trend state.
        /// </summary>
        public bool? TrackSideTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Stopped Out state.
        /// </summary>
        public bool? TrackStoppedOut { get; set; }

        /// <summary>
        /// Trigger alert for the Up Trend state.
        /// </summary>
        public bool? TrackUpTrend { get; set; }

        /// <summary>
        /// Type of the Position Trigger (TrailingStopPercent or VolatilityQuotinent).
        /// </summary>
        public TriggerTypes? TriggerType { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool? UseIntraday { get; set; }

        /// <summary>
        /// Indicates that trigger is chosen as primary for position,
        /// so it will be displayed in corresponding positions grid columns.
        /// </summary>
        public bool? IsPrimary { get; set; }
    }
}
