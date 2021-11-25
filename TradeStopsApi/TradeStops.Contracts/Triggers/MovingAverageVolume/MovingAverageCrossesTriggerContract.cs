using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Moving Average Crosses Trigger that monitors the moving averages of specified times.
    /// (For example, it monitors and notifies when 1 Week Moving Average is 10% Below 1 Month Moving Average.)
    /// </summary>
    public class MovingAverageCrossesTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MovingAverageCrossesTriggerContract()
            : base(TriggerTypes.MovingAverageCrosses)
        {
        }

        /// <summary>
        /// Numeric value of a specific period of time when the moving average has to be equal to the specified percent value.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Type of the trigger Period.
        /// </summary>
        public PeriodTypes PeriodType { get; set; }

        /// <summary>
        /// Specific percent value of the moving average.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time when the moving average has to be equal to the specified percent value to satisfy the trigger operation type .
        /// </summary>
        public int Period2 { get; set; }

        /// <summary>
        /// Type of the trigger Period2.
        /// </summary>
        public PeriodTypes PeriodType2 { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }
    }
}
