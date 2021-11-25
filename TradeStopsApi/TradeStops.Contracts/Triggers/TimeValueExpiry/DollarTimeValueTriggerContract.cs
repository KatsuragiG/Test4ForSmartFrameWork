using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Dollar Time Value Trigger that monitors a specific dollar amount of the option's initial Time Value.
    /// </summary>
    public class DollarTimeValueTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DollarTimeValueTriggerContract()
            : base(TriggerTypes.DollarTimeValue)
        {
        }

        /// <summary>
        /// Dollar amount value of the option's initial Time Value.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }
    }
}
