using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Behavioral event
    /// </summary>
    public class CreateBehavioralEventContract
    {
        /// <summary>
        /// The type of behavioral event
        /// </summary>
        public BehavioralEventTypes BehavioralEventType { get; set; }

        /// <summary>
        /// ID of the product
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// Program argument for event statistics
        /// </summary>
        public int? ProgramArgument { get; set; }
    }
}
