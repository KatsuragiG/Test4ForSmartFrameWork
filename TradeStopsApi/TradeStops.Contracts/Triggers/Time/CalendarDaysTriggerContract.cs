using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Calendar Days After Entry Trigger that monitors a specific number of calendar days after the Entry Date of the position at which to be alerted.
    /// The “Equal” setting will trigger the alert once when the specified number of days is reached.
    /// The “ModuleDeletion” setting will trigger the alert every time when the specified number of days is reached.
    /// </summary>
    public class CalendarDaysTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CalendarDaysTriggerContract()
            : base(TriggerTypes.CalendarDays)
        {
        }

        /// <summary>
        ///  Trigger operation types. Valid values are Equal or ModuleDeletion.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Number of the calendar days after position  entry date .
        /// </summary>
        public int ThresholdValue { get; set; }
    }
}
