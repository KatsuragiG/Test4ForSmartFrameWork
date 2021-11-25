using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create an Above/Below a Moving Average Trigger that monitors the moving average close price for a specified time.
    /// (For example, it monitors and notifies when the Latest Close is 10% below 15 days Moving Average of the Close Price.)
    /// </summary>
    public class MovingAveragePriceTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MovingAveragePriceTriggerContract()
            : base(TriggerTypes.MovingAveragePrice)
        {
        }

        /// <summary>
        /// Trigger price types. Valid values is Close.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Specific percent value of the close price.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Type of the trigger period.
        /// </summary>
        public PeriodTypes PeriodType { get; set; }
    }
}
