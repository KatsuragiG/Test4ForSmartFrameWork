using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Percentage Time Value Trigger that monitors a specific percent of the option's initial Time Value.
    /// </summary>
    public class PercentageTimeValueTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PercentageTimeValueTriggerContract()
            : base(TriggerTypes.PercentageTimeValue)
        {
        }

        /// <summary>
        /// Percent of the option's initial Time Value.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }
    }
}
