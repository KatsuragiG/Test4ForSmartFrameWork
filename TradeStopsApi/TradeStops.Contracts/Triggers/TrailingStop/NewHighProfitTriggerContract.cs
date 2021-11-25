using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// This class can not be used to create physical NewHighProfitTrigger, but is required to map System Events. 
    /// </summary>
    public class NewHighProfitTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NewHighProfitTriggerContract()
            : base(TriggerTypes.NewHighProfit)
        {
        }
    }
}
