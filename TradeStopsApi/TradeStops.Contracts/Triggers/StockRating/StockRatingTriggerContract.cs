using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// This class can not be used to create physical StockRatingTrigger, but is required to map System Events. 
    /// </summary>
    public class StockRatingTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StockRatingTriggerContract()
            : base(TriggerTypes.StockRating)
        {
        }
    }
}
