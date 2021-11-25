using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Behavioral Event
    /// </summary>
    public class BehavioralEventContract
    {
        /// <summary>
        /// Date of the creation
        /// </summary>
        public DateTime TimeSpan { get; set; }

        /// <summary>
        /// The type of behavioral event
        /// </summary>
        public BehavioralEventTypes BehavioralEventType { get; set; }

        /// <summary>
        /// ID of the trade smith user
        /// </summary>
        public int TradeSmithUserId { get; set; }

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
