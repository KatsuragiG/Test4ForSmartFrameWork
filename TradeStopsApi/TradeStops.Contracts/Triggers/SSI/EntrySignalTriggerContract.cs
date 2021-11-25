using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// This class can not be used to create physical EntrySignalTrigger, but is required to map System Events. 
    /// </summary>
    public class EntrySignalTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EntrySignalTriggerContract()
            : base(TriggerTypes.EntrySignal)
        {
        }
    }
}
