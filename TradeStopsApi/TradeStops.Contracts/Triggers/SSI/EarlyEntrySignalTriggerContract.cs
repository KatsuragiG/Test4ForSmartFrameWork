using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// This class can not be used to create physical EarlyEntrySignalTrigger, but is required to map System Events. 
    /// </summary>
    public class EarlyEntrySignalTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EarlyEntrySignalTriggerContract()
            : base(TriggerTypes.EarlyEntrySignal)
        {
        }
    }
}
