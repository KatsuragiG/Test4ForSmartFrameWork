using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Fields required to generate alert state for New Stock Rating trigger
    /// </summary>
    public class StockRatingTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StockRatingTriggerStateContract()
            : base(TriggerTypes.StockRating)
        {
        }

        /// <summary>
        ///  Current global rank value
        /// </summary>
        public GlobalRankTypes CurrentGlobalRank { get; set; }

        /// <summary>
        ///  Previous global rank value
        /// </summary>
        public GlobalRankTypes? PreviousGlobalRank { get; set; }
    }
}
