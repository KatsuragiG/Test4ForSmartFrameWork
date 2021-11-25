using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Stock State Indicator Trigger for a position symbol. Values in the request payload determine the Stock State Indicator changes that will trigger the alert. 
    /// </summary>
    public class StockStateIndicatorTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StockStateIndicatorTriggerContract()
            : base(TriggerTypes.StockStateIndicator)
        {
        }        
    }
}
