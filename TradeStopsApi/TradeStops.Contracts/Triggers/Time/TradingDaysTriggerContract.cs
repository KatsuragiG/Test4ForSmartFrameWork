using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Trading Days After Entry Trigger that monitors a specific number of trading days after the Entry Date of the position.
    /// Trading days are the days the market is open.
    /// The “Equal” setting will trigger the alert once when the specified number of trading days is reached.
    /// The “ModuleDeletion” setting will trigger the alert every time when the specified number of trading days is reached.
    /// </summary>
    public class TradingDaysTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TradingDaysTriggerContract()
            : base(TriggerTypes.TradingDays)
        {
        }

        /// <summary>
        /// Trigger operation types. Valid values are Equal or ModuleDeletion.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Number of the position trigger days .
        /// </summary>
        public int ThresholdValue { get; set; }
    }
}
