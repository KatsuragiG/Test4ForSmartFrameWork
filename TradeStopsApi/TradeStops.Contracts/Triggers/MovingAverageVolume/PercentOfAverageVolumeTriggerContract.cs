using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Percent Of Average Volume Trigger that monitors the latest closing volume in relation to the average volume for a specified time.
    /// (For example, it monitors and notifies when  the latest volume is 10% below 2 week average volume.)
    /// </summary>
    public class PercentOfAverageVolumeTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PercentOfAverageVolumeTriggerContract()
            : base(TriggerTypes.PercentOfAverageVolume)
        {
        }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Specific percent value compared to the average volume.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Type of the trigger period.
        /// </summary>
        public PeriodTypes PeriodType { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }
    }
}
